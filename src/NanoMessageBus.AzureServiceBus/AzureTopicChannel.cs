using System;
using System.Linq;
using System.Threading;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class AzureTopicChannel : IMessagingChannel
    {
        private readonly ServiceBusConnector _connector;
        private readonly ServiceBusChannelGroupConfiguration _configuration;
        private readonly CancellationTokenSource _shutdownReceiver =
            new CancellationTokenSource();
        private TopicClient _topicClient;
        private SubscriptionClient _subscriptionClient;
        private bool _isShutdown;

        public AzureTopicChannel(
            ServiceBusConnector connector,
            ServiceBusChannelGroupConfiguration configuration,
            TopicClient topicClient,
            SubscriptionClient subscriptionClient)
        {
            _connector = connector;
            _configuration = configuration;
            _topicClient = topicClient;
            _subscriptionClient = subscriptionClient;

            CurrentResolver = configuration.DependencyResolver;
            CurrentTransaction = null;

            _connector.CurrentState = ConnectionState.Open;
            Active = true;
        }

        public bool Active { get; private set; }

        public IChannelGroupConfiguration CurrentConfiguration => _configuration;

        public ChannelMessage CurrentMessage { get; private set; }

        public IDependencyResolver CurrentResolver { get; private set; }

        /// <summary>
        /// Gets the current transaction.
        /// </summary>
        /// <remarks>
        /// Transactions are not supported on Azure ServiceBus so this property always returns <c>null.</c>
        /// </remarks>
        public IChannelTransaction CurrentTransaction { get; private set; }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Send(ChannelEnvelope envelope)
        {
            if (_isShutdown)
            {
                throw new InvalidOperationException("Channel has shutdown.");
            }

            var message = _configuration.MessageAdapter.Build(envelope.Message);
            if (message.Length == 1)
            {
                _topicClient.SendAsync(message.First());
            }
            else
            {
                _topicClient.SendBatchAsync(message);
            }
        }

        public async void Receive(Action<IDeliveryContext> callback)
        {
            if (_configuration.DispatchOnly)
            {
                throw new InvalidOperationException("Receive is not valid on dispatch only channel.");
            }

            while (!_shutdownReceiver.IsCancellationRequested)
            {
                BrokeredMessage message = null;
                try
                {
                    message = await _subscriptionClient
                        .PeekAsync().ConfigureAwait(false);

                    Receive(message, callback);

                    await _subscriptionClient
                        .CompleteAsync(message.LockToken)
                        .ConfigureAwait(false);
                }
                catch
                {
                    if (message != null)
                    {
                        await _subscriptionClient
                            .DeadLetterAsync(message.LockToken)
                            .ConfigureAwait(false);
                    }
                }
            }

            _connector.CurrentState = ConnectionState.Closed;
        }

        public IDispatchContext PrepareDispatch(object message = null, IMessagingChannel channel = null)
        {
            var context = new DefaultDispatchContext(channel ?? this);
            return message == null ? context : context.WithMessage(message);
        }

        public void BeginShutdown()
        {
            _isShutdown = true;
            Active = false;
            _connector.CurrentState = ConnectionState.Closing;

            if (!_configuration.DispatchOnly)
            {
                _shutdownReceiver.Cancel();
            }
        }

        public virtual void Receive(
            BrokeredMessage message,
            Action<IDeliveryContext> callback)
        {
            CurrentMessage = null;
            try
            {
                CurrentMessage = _configuration.MessageAdapter.Build(message);
                callback(this);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_subscriptionClient != null)
                {
                    _subscriptionClient.Close();
                    _subscriptionClient = null;
                }

                if (_topicClient != null)
                {
                    _topicClient.Close();
                    _topicClient = null;
                }
            }
        }
    }
}