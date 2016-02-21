using System;
using System.Collections.Generic;
using System.Linq;

namespace NanoMessageBus.Channels
{
    public class ServiceBusConnector : IChannelConnector
    {
        private readonly ICollection<ServiceBusChannelGroupConfiguration> _configurations;
        private readonly AzureTopicConnectionFactory _connectionFactory;

        public ServiceBusConnector(
            AzureTopicConnectionFactory connectionFactory,
            ICollection<ServiceBusChannelGroupConfiguration> configurations)
        {
            _connectionFactory = connectionFactory;
            _configurations = configurations;
        }

        public ConnectionState CurrentState { get; internal set; }

        public IEnumerable<IChannelGroupConfiguration> ChannelGroups => _configurations;

        public void Dispose()
        {
        }

        public IMessagingChannel Connect(string channelGroup)
        {
            var config = _configurations.FirstOrDefault(c => c.GroupName == channelGroup);
            if (config == null)
            {
                throw new ArgumentException("Channel group not found.", nameof(channelGroup));
            }

            return _connectionFactory.CreateConnection(this, config);
        }
    }
}
