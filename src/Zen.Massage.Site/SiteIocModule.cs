using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Microsoft.ApplicationInsights;
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
            var eventHubConnector = new EventHubWireup()
                .WithHubConnectionString(Configuration["MessageBus:EventHubConnectionString"])
                .WithStoreConnectionString(Configuration["MessageBus:StorageConnectionString"])
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
            private readonly IMessagingHost _messagingHost;
            private IChannelGroup _channelGroup;
            private IMessagingChannel _messageChannel;

            public PipelineDispatcherHook(
                IMessagingHost messagingHost)
            {
                _messagingHost = messagingHost;
                _channelGroup = _messagingHost.Initialize();
                _messageChannel = _channelGroup.OpenChannel();
            }

            public void Dispose()
            {
                Dispose(true);
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
                var messageId = Guid.NewGuid();
                var correlationId = Guid.NewGuid();
                var message = new ChannelMessage(
                    messageId,
                    correlationId,
                    new Uri("uri://noreply.zenmassage.com"),
                    new Dictionary<string, string>(),
                    committed.Events.Select(e => e.Body));
                var envelope = new ChannelEnvelope(
                    message,
                    new[]
                    {
                       new Uri("uri://domain.zenmassage.com"),  
                    });
                _messageChannel.Send(envelope);
            }

            public void OnPurge(string bucketId)
            {
            }

            public void OnDeleteStream(string bucketId, string streamId)
            {
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (_messageChannel != null)
                    {
                        _messageChannel.Dispose();
                        _messageChannel = null;
                    }
                    if (_channelGroup != null)
                    {
                        _channelGroup.Dispose();
                        _channelGroup = null;
                    }
                }
            }
        }
    }
}
