using System;

namespace NanoMessageBus.Channels
{
    public class EventHubChannelGroupConfiguration : IChannelGroupConfiguration
    {
        public string GroupName { get; }

        public bool Synchronous { get; }

        public bool DispatchOnly { get; }

        public int MaxDispatchBuffer { get; }

        public int MinWorkers { get; }

        public int MaxWorkers { get; }

        public Uri ReturnAddress { get; }

        public IChannelMessageBuilder MessageBuilder { get; }

        public TimeSpan ReceiveTimeout { get; }

        public IDependencyResolver DependencyResolver { get; }

        public IDispatchTable DispatchTable { get; }
    }
}