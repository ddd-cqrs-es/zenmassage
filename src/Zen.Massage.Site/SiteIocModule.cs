using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.ApplicationInsights;
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
        private readonly bool _useInMemoryPersistence;

        public SiteIocModule()
        {
            _useInMemoryPersistence = true;
        }

        // TODO: Pass ASP.NET configuration object into module
        //  so setup parameters can be setup via IoC

        protected override void Load(ContainerBuilder builder)
        {
            // Setup messagebus (we're using lightweight message bus rather than Azure ServiceBus)
            var routingTable =
                new AutofacRoutingTable(
                    builder,
                    Assembly.GetExecutingAssembly(),
                    typeof(BookingUpdater).Assembly);
            var messagingHost = new MessagingWireup()
                .AddConnector(null)
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
                .As<IBookingReadRepository>();
            builder.RegisterType<IBookingWriteRepository>()
                .As<IBookingWriteRepository>();

            // Register application types
            builder.RegisterType<BookingCommandService>()
                .As<IBookingCommandService>();

            // Register site types
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
            PersistenceWireup wireup;
            if (_useInMemoryPersistence)
            {
                wireup = Wireup.Init()
                    .UsingInMemoryPersistence();
            }
            else
            {
                wireup = Wireup.Init()
                    .UsingSqlPersistence("EventStore")
                    .InitializeStorageEngine();
            }

            // Setup remainder of neventstore and build
            return wireup
                .UsingJsonSerialization()
                .Compress()
                .HookIntoPipelineUsing(new PipelineDispatcherHook(container))
                .Build();
        }

        public class ApplicationInsightsMessageAuditor : IMessageAuditor
        {
            private readonly IMessagingChannel _channel;
            private readonly TelemetryClient _telemetryClient;

            public ApplicationInsightsMessageAuditor(IMessagingChannel channel)
            {
                _channel = channel;
                _telemetryClient = new TelemetryClient();
            }

            public void Dispose()
            {
            }

            public void AuditReceive(IDeliveryContext delivery)
            {
                _telemetryClient.TrackEvent("ReceiveMessage",
                    new Dictionary<string, string>
                    {
                        { "ChannelGroup", _channel.CurrentConfiguration.GroupName },
                        { "MessageType", delivery.CurrentMessage.ActiveMessage.GetType().Name }
                    });
            }

            public void AuditSend(ChannelEnvelope envelope, IDeliveryContext delivery)
            {
                _telemetryClient.TrackEvent("SendMessage",
                    new Dictionary<string, string>
                    {
                        { "ChannelGroup", _channel.CurrentConfiguration.GroupName },
                        { "MessageType", envelope.Message.ActiveMessage.GetType().Name }
                    });
            }
        }

        public class PipelineDispatcherHook : IPipelineHook
        {
            private readonly ILifetimeScope _container;

            public PipelineDispatcherHook(ILifetimeScope container)
            {
                _container = container;
            }

            public void Dispose()
            {
            }

            public ICommit Select(ICommit committed)
            {
                return committed;
            }

            public bool PreCommit(CommitAttempt attempt)
            {
                return true;
            }

            public void PostCommit(ICommit committed)
            {
                using (var scope = _container.BeginLifetimeScope())
                {
                    // Get dispatch context
                    var publisher = scope.Resolve<IDispatchContext>();

                    // Publish events and commit
                    publisher
                        .Publish(committed.Events.Select(e => e.Body).ToArray())
                        .Commit();
                }
            }

            public void OnPurge(string bucketId)
            {
            }

            public void OnDeleteStream(string bucketId, string streamId)
            {
            }
        }
    }
}
