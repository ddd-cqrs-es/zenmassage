using System.Collections.Generic;
using System.Reflection;
using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NanoMessageBus;
using NanoMessageBus.Channels;
using NEventStore;
using Zen.Infrastructure.ReadRepository;
using Zen.Infrastructure.WriteRepository;
using Zen.Massage.Application;
using Zen.Massage.Domain;
using Zen.Massage.Domain.BookingContext;
using Module = Autofac.Module;

namespace Zen.Massage.Site
{
    public class SiteIocModule : Module
    {
        private readonly IConfigurationRoot _configuration;
        private readonly bool _useInMemoryPersistence;

        public SiteIocModule(IConfigurationRoot configuration)
        {
            _configuration = configuration;
            _useInMemoryPersistence = true;
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

            // Setup messagebus (we're using lightweight message bus rather than Azure ServiceBus)
            var routingTable =
                new AutofacRoutingTable(
                    builder,
                    Assembly.GetExecutingAssembly(),
                    typeof(BookingUpdater).Assembly);
            var eventHubConnector = new ServiceBusWireup()
                .WithConnectionString(Configuration["MessageBus:ServiceBusConnectionString"])
                .AddChannelGroup(
                    config => config
                        .WithInputHubPath("domainevents")
                        .WithGroupName("default")
                    )
                .Build();
            var messagingHost = new MessagingWireup()
                .AddConnector(eventHubConnector)
                .WithAuditing(GetAuditorsForChannel)
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
            // Setup the appropriate persistence layer
            // TODO: Setup to use Azure Blob Storage for persistence
            //  based on work by Chris Evans (already forked repo)
            PersistenceWireup wireup;
            if (_useInMemoryPersistence)
            {
                wireup = Wireup.Init()
                    .UsingInMemoryPersistence();
            }
            else
            {
                wireup = Wireup.Init()
                    .UsingSqlPersistence("EventStore", "SqlClient", Configuration["EventStore:Database"])
                    .InitializeStorageEngine();
            }

            // Setup remainder of neventstore and build
            return wireup
                .UsingJsonSerialization()
                .Compress()
                .HookIntoPipelineUsing(container.Resolve<PipelineDispatcherHook>())
                .Build();
        }
    }
}
