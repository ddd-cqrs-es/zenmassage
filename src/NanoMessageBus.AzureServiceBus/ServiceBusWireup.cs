using System;
using System.Collections.Generic;

namespace NanoMessageBus.Channels
{
    public class ServiceBusWireup
    {
        private readonly AzureTopicConnectionFactory _channelFactory =
            new AzureTopicConnectionFactory();
        private readonly List<ServiceBusChannelGroupConfiguration> _configurations =
            new List<ServiceBusChannelGroupConfiguration>(); 

        public ServiceBusWireup WithConnectionString(string connectionString)
        {
            _channelFactory.SetConnectionString(connectionString);
            return this;
        }

        public ServiceBusWireup AddChannelGroup(Action<ServiceBusChannelGroupConfiguration> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var config = new ServiceBusChannelGroupConfiguration();
            callback(config);
            _configurations.Add(config);
            return this;
        }

        public ServiceBusConnector Build()
        {
            return new ServiceBusConnector(_channelFactory, _configurations);
        }
    }
}