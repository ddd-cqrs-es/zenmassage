using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class ChannelEventProcessor : IEventProcessor
    {
        private readonly EventHubChannel _owner;
        private readonly Action<IDeliveryContext> _deliveryCallback;

        public ChannelEventProcessor(EventHubChannel owner, Action<IDeliveryContext> deliveryCallback)
        {
            _owner = owner;
            _deliveryCallback = deliveryCallback;
        }

        public Task OpenAsync(PartitionContext context)
        {
            return Task.FromResult(true);
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var message in messages)
            {
                _owner.Receive(context, message, _deliveryCallback);
            }

            return Task.FromResult(true);
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            return Task.FromResult(true);
        }
    }
}