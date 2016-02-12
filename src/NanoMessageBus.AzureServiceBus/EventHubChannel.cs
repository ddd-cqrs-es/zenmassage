using System;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class EventHubChannel : IMessagingChannel
    {
        private readonly IChannelConnector _connector;
        private readonly EventHubChannelGroupConfiguration _configuration;
        private EventHubClient _eventHubClient;
        private EventProcessorHost _eventProcessorHost;

        public EventHubChannel(
            IChannelConnector connector,
            EventHubChannelGroupConfiguration configuration,
            EventHubClient eventHubClient,
            EventProcessorHost eventProcessorHost)
        {
            _connector = connector;
            _configuration = configuration;
            _eventHubClient = eventHubClient;
            _eventProcessorHost = eventProcessorHost;

            CurrentResolver = configuration.DependencyResolver;
            CurrentTransaction = new EventHubTransaction();
        }

        public bool Active { get; private set; }

        public IChannelGroupConfiguration CurrentConfiguration => _configuration;

        public ChannelMessage CurrentMessage { get; private set; }

        public IDependencyResolver CurrentResolver { get; private set; }

        public IChannelTransaction CurrentTransaction { get; private set; }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Send(ChannelEnvelope envelope)
        {
            var message = _configuration.MessageAdapter.Build(envelope.Message);
            _eventHubClient.Send(message);
        }

        public async void Receive(Action<IDeliveryContext> callback)
        {
            if (_configuration.DispatchOnly)
            {
                throw new InvalidOperationException("Receive is not valid on dispatch only channel.");
            }

            await _eventProcessorHost
                .RegisterEventProcessorFactoryAsync(new ChannelEventProcessorFactory(this, callback))
                .ConfigureAwait(false);
        }

        public virtual void Receive(
            PartitionContext partitionContext,
            EventData message,
            Action<IDeliveryContext> callback)
        {
            CurrentMessage = null;
            EnsureTransaction();
            TryReceive(partitionContext, message, callback);
        }

        public virtual void TryReceive(
            PartitionContext partitionContext,
            EventData message,
            Action<IDeliveryContext> callback)
        {
            var messageId = message.SequenceNumber;
            try
            {
                CurrentMessage = _configuration.MessageAdapter.Build(message);
                callback(this);
            }
            catch (Exception)
            {
                CurrentTransaction.TryDispose();
                throw;
            }
        }

        public IDispatchContext PrepareDispatch(object message = null, IMessagingChannel channel = null)
        {
            EnsureTransaction();
            var context = new DefaultDispatchContext(channel ?? this);
            return message == null ? context : context.WithMessage(message);
        }

        public void BeginShutdown()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        protected virtual IChannelTransaction EnsureTransaction()
        {
            if (!CurrentTransaction.Finished)
            {
                return CurrentTransaction;
            }

            CurrentTransaction.TryDispose();
            return CurrentTransaction = new EventHubTransaction();
        }
    }
}