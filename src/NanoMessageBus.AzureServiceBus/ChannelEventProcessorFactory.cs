using System;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class ChannelEventProcessorFactory : IEventProcessorFactory
    {
        private readonly EventHubChannel _owner;
        private readonly Action<IDeliveryContext> _deliveryCallback;

        public ChannelEventProcessorFactory(EventHubChannel owner, Action<IDeliveryContext> deliveryCallback)
        {
            _owner = owner;
            _deliveryCallback = deliveryCallback;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new ChannelEventProcessor(_owner, _deliveryCallback);
        }
    }
}