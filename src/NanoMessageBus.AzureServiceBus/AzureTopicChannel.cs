using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using NanoMessageBus.Logging;

namespace NanoMessageBus.Channels
{
    public class AzureTopicChannel : IMessagingChannel
    {
        private static readonly ILog Log = LogFactory.Build(typeof(AzureTopicChannel));
        private static int _nextIdentifier = 1;

        private readonly ServiceBusConnector _connector;
        private readonly ServiceBusChannelGroupConfiguration _configuration;
        private readonly CancellationTokenSource _shutdownToken =
            new CancellationTokenSource();
        private CancellationTokenRegistration _masterShutdownTokenRegistration;
        private TopicClient _topicClient;
        private SubscriptionClient _subscriptionClient;
        private int _identifier;
        private bool _isShutdown;
        private bool _isDisposed;

        public AzureTopicChannel(
            ServiceBusConnector connector,
            ServiceBusChannelGroupConfiguration configuration,
            TopicClient topicClient,
            SubscriptionClient subscriptionClient,
            CancellationToken shutdownToken)
        {
            _connector = connector;
            _configuration = configuration;
            _topicClient = topicClient;
            _subscriptionClient = subscriptionClient;
            _masterShutdownTokenRegistration = shutdownToken
                .Register(() => _shutdownToken.Cancel());
            _identifier = Interlocked.Increment(ref _nextIdentifier);

            CurrentResolver = configuration.DependencyResolver;
            CurrentTransaction = null;
        }

        ~AzureTopicChannel()
        {
            Dispose(false);
        }

        public bool Active => !_isDisposed && !_isShutdown && _connector.CurrentState == ConnectionState.Open;

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
            GC.SuppressFinalize(this);
        }

        public async void Send(ChannelEnvelope envelope)
        {
            if (_isShutdown)
            {
                throw new InvalidOperationException("Channel has shutdown.");
            }

            var message = _configuration.MessageAdapter.Build(envelope.Message);
            await _topicClient.SendAsync(message).ConfigureAwait(false);
        }

        public void Receive(Action<IDeliveryContext> callback)
        {
            if (_configuration.DispatchOnly)
            {
                throw new InvalidOperationException("Receive is not valid on dispatch only channel.");
            }

            while (!_shutdownToken.IsCancellationRequested)
            {
                try
                {
                    BrokeredMessage message = _subscriptionClient.PeekAsync().Result;
                    var result = ReceiveAsync(message, callback).Result;
                }
                catch(Exception e)
                {
                    Log.Warn($"Exception caught in Receive message loop - [{e.Message}]", e);
                }
            }
        }

        public IDispatchContext PrepareDispatch(object message = null, IMessagingChannel channel = null)
        {
            var context = new DefaultDispatchContext(channel ?? this);
            return message == null ? context : context.WithMessage(message);
        }

        public void BeginShutdown()
        {
            _isShutdown = true;

            if (!_configuration.DispatchOnly)
            {
                _shutdownToken.Cancel();
            }
        }

        protected virtual async Task<bool> ReceiveAsync(
            BrokeredMessage message,
            Action<IDeliveryContext> callback)
        {
            CurrentMessage = null;

            if (_isShutdown)
            {
                Log.Debug($"Shutdown request has been made on channel {_identifier}.");
                return false;
            }

            if (message == null)
            {
                return true;
            }

            await TryReceiveAsync(message, callback).ConfigureAwait(false);

            return !_isShutdown;
        }

        protected virtual async Task TryReceiveAsync(
            BrokeredMessage message,
            Action<IDeliveryContext> callback)
        {
            try
            {
                Log.Verbose($"Translating wire-specific message into channel message for channel {_identifier}.");
                CurrentMessage = _configuration.MessageAdapter.Build(message);

                Log.Info($"Routing message '{message.MessageId}' received through group '{_configuration.GroupName}' to configured receiver callback on channel {_identifier}.");
                callback(this);

                // Signal message processing as having completed.
                await _subscriptionClient
                    .CompleteAsync(message.LockToken)
                    .ConfigureAwait(false);
            }
            catch (ChannelConnectionException)
            {
                Log.Warn($"Channel {_identifier} has become unavailable, aborting.");
                throw;
            }
            catch (PoisonMessageException)
            {
                Log.Warn($"Wire message {message.MessageId} on channel {_identifier} could not be deserialized; forwarding to poison message exchange.");

                await _subscriptionClient
                    .DeadLetterAsync(message.LockToken)
                    .ConfigureAwait(false);
            }
            catch (DeadLetterException e)
            {
                var seconds = (SystemTime.UtcNow - e.Expiration).TotalSeconds;
                Log.Info($"Wire message {message.MessageId} on channel {_identifier} expired on the wire {seconds:n3} seconds ago; forwarding to dead letter exchange.");

                await _subscriptionClient
                    .DeadLetterAsync(message.LockToken)
                    .ConfigureAwait(false);
            }
            catch (Exception)
            {
                
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                Log.Debug($"Disposing channel {_identifier}.");

                _isDisposed = true;
                _masterShutdownTokenRegistration.Dispose();

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

                Log.Debug($"Channel {_identifier} disposed.");
            }
        }
    }
}