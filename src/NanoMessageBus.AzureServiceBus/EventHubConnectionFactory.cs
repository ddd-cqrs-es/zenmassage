using System;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class EventHubConnectionFactory
    {
        private string _hubConnectionString;
        private string _storeConnectionString;

        public void SetHubConnectionString(string hubConnectionString)
        {
            _hubConnectionString = hubConnectionString;
        }

        public void SetStoreConnectionString(string storeConnectionString)
        {
            _storeConnectionString = storeConnectionString;
        }

        public IMessagingChannel CreateConnection(
            IChannelConnector connector,
            EventHubChannelGroupConfiguration config)
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(_hubConnectionString, config.InputHubPath);
            EventProcessorHost host = null;
            if (!config.DispatchOnly)
            {
                var consumerGroup = eventHubClient.GetDefaultConsumerGroup();
                if (!string.IsNullOrWhiteSpace(config.GroupName) && config.GroupName != "default")
                {
                    consumerGroup = eventHubClient.GetConsumerGroup(config.GroupName);
                }

                host = new EventProcessorHost(
                    consumerGroup.EventHubPath,
                    consumerGroup.GroupName,
                    _hubConnectionString,
                    _storeConnectionString);
            }

            return new EventHubChannel(connector, config, eventHubClient, host);
        }
    }
}