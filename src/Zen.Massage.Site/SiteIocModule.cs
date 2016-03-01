using System.Collections.Generic;
using System.Reflection;
using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NanoMessageBus;
using NanoMessageBus.Channels;
using NanoMessageBus.Logging;
using NEventStore;
using NEventStore.Persistence.AzureStorage;
using Zen.Infrastructure.ReadRepository;
using Zen.Infrastructure.WriteRepository;
using Zen.Massage.Application;
using Zen.Massage.Domain;
using Zen.Massage.Domain.BookingBoundedContext;
using Module = Autofac.Module;

namespace Zen.Massage.Site
{
    public class SiteIocModule : Module
    {
        private readonly IConfigurationRoot _configuration;

        public SiteIocModule(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public IConfigurationRoot Configuration => _configuration;

        protected override void Load(ContainerBuilder builder)
        {
            // Setup automapper configuration
            var mapperConfig = new MapperConfiguration(
                configure =>
                {
                    configure.AddProfile<AutomapperConfigProfile>();
                });
            builder.RegisterInstance(mapperConfig);

            // Setup dependency resolver
            builder
                .Register(c => new AutofacDependencyResolver(c.Resolve<ILifetimeScope>()))
                .As<IDependencyResolver>();

            // Setup messagebus (we're using lightweight message bus rather than Azure ServiceBus)
            var routingTable =
                new AutofacRoutingTable(
                    builder,
                    Assembly.GetExecutingAssembly(),
                    typeof(BookingUpdater).Assembly);
            var serviceBusConnector = new ServiceBusWireup()
                .WithConnectionString(Configuration["MessageBus:ServiceBusConnectionString"])
                .AddChannelGroup(
                    config => config
                        .WithInputHubPath("domainevents")
                        .WithDispatchTable(new ServiceBusDispatchTable())
                        //.WithDependencyResolver(new AutofacDependencyResolver())
                        .WithGroupName("default")
                    )
                .Build();
            LogFactory.LogWith(new ApplicationInsightsMessagingLogger());
            var messagingHost = new MessagingWireup()
                .AddConnector(serviceBusConnector)
                //.WithAuditing(GetAuditorsForChannel)
                .StartWithReceive(routingTable);
            builder.RegisterInstance(messagingHost);

            // Setup event store
            builder.Register(c => BuildEventStore(c.Resolve<ILifetimeScope>()))
                .As<IStoreEvents>()
                .SingleInstance();

            // Register core domain types
            builder.RegisterType<UnitOfWorkFactory>()
                .As<IUnitOfWorkFactory>();
            builder.RegisterType<UnitOfWorkSession>();
            builder.RegisterType<BookingFactory>()
                .As<IBookingFactory>();

            // Register infrastructure types
            builder.RegisterType<BookingReadRepository>()
                .WithParameter("connectionString", Configuration["ReadStore:DatabaseConnectionString"])
                .As<IBookingReadRepository>();
            builder.RegisterType<BookingWriteRepository>()
                .As<IBookingWriteRepository>();

            // Register application types
            builder.RegisterType<BookingCommandService>()
                .As<IBookingCommandService>();

            // Register site types
            builder.RegisterType<PipelineDispatcherHook>();
        }

        private IEnumerable<IMessageAuditor> GetAuditorsForChannel(IMessagingChannel channel)
        {
            return new[]
            {
                new ApplicationInsightsMessageAuditor(channel)
            };
        }

        private IStoreEvents BuildEventStore(ILifetimeScope container)
        {
            // Setup to use Azure Blob Storage for persistence
            return Wireup.Init()
                .UsingAzureBlobPersistence(
                    Configuration["MessageBus:StorageConnectionString"],
                    new AzureBlobPersistenceOptions("eventStore"))
                .UsingJsonSerialization()
                .Compress()
                .HookIntoPipelineUsing(container.Resolve<PipelineDispatcherHook>())
                .Build();
        }
    }
}
