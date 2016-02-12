using System;

namespace NanoMessageBus.Channels
{
    public class EventHubChannel : IMessagingChannel
    {
        public bool Active { get; }

        public ChannelMessage CurrentMessage { get; }

        public IDependencyResolver CurrentResolver { get; }

        public IChannelTransaction CurrentTransaction { get; }

        public IChannelGroupConfiguration CurrentConfiguration { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Send(ChannelEnvelope envelope)
        {
            throw new NotImplementedException();
        }

        public void Receive(Action<IDeliveryContext> callback)
        {
            throw new NotImplementedException();
        }

        public IDispatchContext PrepareDispatch(object message = null, IMessagingChannel channel = null)
        {
            throw new NotImplementedException();
        }

        public void BeginShutdown()
        {
            throw new NotImplementedException();
        }
    }
}