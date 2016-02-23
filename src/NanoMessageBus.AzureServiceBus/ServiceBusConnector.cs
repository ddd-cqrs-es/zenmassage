using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NanoMessageBus.Logging;

namespace NanoMessageBus.Channels
{
    public class ServiceBusConnector : IChannelConnector
    {
        private static readonly ILog Log = LogFactory.Build(typeof (ServiceBusConnector));
        private readonly IDictionary<string, ServiceBusChannelGroupConfiguration> _configurations;
        private readonly AzureTopicConnectionFactory _connectionFactory;
        private readonly CancellationTokenSource _shutdownTokenSource = new CancellationTokenSource();
        private bool _isDisposed;

        public ServiceBusConnector(
            AzureTopicConnectionFactory connectionFactory,
            IEnumerable<ServiceBusChannelGroupConfiguration> configurations)
        {
            _connectionFactory = connectionFactory;
            _configurations = (configurations ?? new ServiceBusChannelGroupConfiguration[0])
                .Where(x => x != null)
                .Where(x => !string.IsNullOrEmpty(x.GroupName))
                .ToDictionary(x => x.GroupName ?? string.Empty, x => x);

            if (_configurations.Count == 0)
            {
                throw new ArgumentException("No configurations provided.", nameof(configurations));
            }
        }

        ~ServiceBusConnector()
        {
            Dispose(false);
        }

        public ConnectionState CurrentState { get; private set; }

        public IEnumerable<IChannelGroupConfiguration> ChannelGroups => _configurations.Values;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IMessagingChannel Connect(string channelGroup)
        {
            try
            {
                CurrentState = ConnectionState.Opening;

                var config = GetChannelGroupConfiguration(channelGroup);
                var channel = _connectionFactory.CreateConnection(this, config);

                CurrentState = ConnectionState.Open;
                return channel;
            }
            catch (Exception)
            {
                CurrentState = ConnectionState.Closed;
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                CurrentState = ConnectionState.Closing;

                _shutdownTokenSource.Cancel();

                CurrentState = ConnectionState.Closed;
                _isDisposed = true;
            }
        }

        protected virtual ServiceBusChannelGroupConfiguration GetChannelGroupConfiguration(string channelGroup)
        {
            if (string.IsNullOrEmpty(channelGroup))
            {
                throw new ArgumentNullException(nameof(channelGroup));
            }

            ServiceBusChannelGroupConfiguration config;
            if (!_configurations.TryGetValue(channelGroup, out config))
            {
                throw new KeyNotFoundException("Channel configuration not found.");
            }

            return config;
        }
    }
}
