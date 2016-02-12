using System;
using System.Collections.Generic;

namespace NanoMessageBus.Channels
{
    public class EventHubConnector : IChannelConnector
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IMessagingChannel Connect(string channelGroup)
        {
            throw new NotImplementedException();
        }

        public ConnectionState CurrentState { get; }
        public IEnumerable<IChannelGroupConfiguration> ChannelGroups { get; }
    }
}
