using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using Swashbuckle.SwaggerGen;
using Swashbuckle.SwaggerGen.XmlComments;

namespace Zen.Massage.Site
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc();

            // Setup swagger integration
            // NOTE: To avoid disclosure of directory structure and ease deployment
            //  fetch doc path from user-secret config (dev env only)
            var documentationPath = Configuration.Get<string>("swagger:xmlpath");
            services.AddSwaggerGen();
            services.ConfigureSwaggerDocument(
                options =>
                {
                    options.SecurityDefinitions["apiKey"] =
                        new ApiKeyScheme
                        {
                            Name = "api-key",
                            Description = "API Key Authentication",
                            In = "header",
                            Type = "apiKey"
                        };
                    options.MultipleApiVersions(
                        new[]
                        {
                            new Info
                            {
                                Version = "v1",
                                Title = "Zen Massage SDK (v1)",
                                Description = "Application Programming Interface (API) for interacting with Zen Massage services.",
                                Contact =
                                    new Contact
                                    {
                                        Name = "Developer Support",
                                        Email = "support@somedomainorother.com",
                                        Url = "http://www.somedomainorother.com"
                                    },
                                TermsOfService = "None"
                            }/*,
                            new Info
                            {
                                Version = "v2",
                                Title = "Zen Massage SDK (v2)",
                                Description = "Application Programming Interface (API) for interacting with Zen Massage services.",
                                Contact =
                                    new Contact
                                    {
                                        Name = "Developer Support",
                                        Email = "support@somedomainorother.com",
                                        Url = "http://www.somedomainorother.com"
                                    },
                                TermsOfService = "None"
                            }*/
                        },
                        (apiDesc, version) => apiDesc.GroupName.EndsWith(version, StringComparison.OrdinalIgnoreCase));
                    options.OperationFilter<SwaggerRemoveCancellationTokenParameterFilter>();
                    options.OperationFilter(new ApplyXmlActionComments(documentationPath));
                });
            services.ConfigureSwaggerSchema(
                options =>
                {
                    options.ModelFilter(new ApplyXmlTypeComments(documentationPath));
                });

            // Setup autofac dependency injection
            var module = new SiteIocModule(Configuration);
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            builder.Populate(services);
            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();

            app.UseCookieAuthentication(
                options =>
                {
                    options.AutomaticAuthenticate = true;
                });

            app.UseOpenIdConnectAuthentication(
                options =>
                {
                    options.AutomaticChallenge = true;
                    options.ClientId = Configuration["Authentication:AzureAd:ClientId"];
                    options.Authority = Configuration["Authentication:AzureAd:AADInstance"] + Configuration["Authentication:AzureAd:TenantId"];
                    options.PostLogoutRedirectUri = Configuration["Authentication:AzureAd:PostLogoutRedirectUri"];
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                });

            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });

            app.UseSwaggerGen();
            app.UseSwaggerUi();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

    public class SwaggerRemoveCancellationTokenParameterFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            context.ApiDescription.ParameterDescriptions
                .Where(pd =>
                    pd.ModelMetadata.ContainerType == typeof(CancellationToken) ||
                    pd.ModelMetadata.ContainerType == typeof(WaitHandle) ||
                    pd.ModelMetadata.ContainerType == typeof(SafeWaitHandle))
                .ToList()
                .ForEach(
                    pd =>
                    {
                        if (operation.Parameters != null)
                        {
                            var cancellationTokenParameter = operation.Parameters.Single(p => p.Name == pd.Name);
                            operation.Parameters.Remove(cancellationTokenParameter);
                        }
                    });
        }
    }
}
