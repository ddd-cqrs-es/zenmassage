using System;
using System.Collections.Generic;

namespace NanoMessageBus.Channels
{
    public class EventHubWireup
    {
        private EventHubConnectionFactory _channelFactory =
            new EventHubConnectionFactory();
        private List<EventHubChannelGroupConfiguration> _channelGroupConfigurations =
            new List<EventHubChannelGroupConfiguration>(); 

        public EventHubWireup WithHubConnectionString(string hubConnectionString)
        {
            _channelFactory.SetHubConnectionString(hubConnectionString);
            return this;
        }

        public EventHubWireup WithStoreConnectionString(string storeConnectionString)
        {
            _channelFactory.SetStoreConnectionString(storeConnectionString);
            return this;
        }

        public EventHubWireup AddChannelGroup(Action<EventHubChannelGroupConfiguration> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var config = new EventHubChannelGroupConfiguration();
            callback(config);
            _channelGroupConfigurations.Add(config);
            return this;
        }
    }
}