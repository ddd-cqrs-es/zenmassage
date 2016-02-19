using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class ChannelEventProcessor : IEventProcessor
    {
        private readonly EventHubChannel _owner;
        private readonly Action<IDeliveryContext> _deliveryCallback;
        private readonly ConcurrentDictionary<string, PartitionContext> _partitions =
            new ConcurrentDictionary<string, PartitionContext>();
        private long? _lastSequenceNumber = null;

        public ChannelEventProcessor(EventHubChannel owner, Action<IDeliveryContext> deliveryCallback)
        {
            _owner = owner;
            _deliveryCallback = deliveryCallback;
        }

        public Task OpenAsync(PartitionContext context)
        {
            _partitions.TryAdd(context.Lease.PartitionId, context);
            return Task.FromResult(true);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var message in messages)
            {
                while (_lastSequenceNumber != null && _lastSequenceNumber + 1 == message.SequenceNumber)
                {
                    Thread.Sleep(10);
                }

                _lastSequenceNumber = message.SequenceNumber - 1;
                _owner.Receive(context, message, _deliveryCallback);
                _lastSequenceNumber = message.SequenceNumber;

                await context.CheckpointAsync(message).ConfigureAwait(false);
            }

            await context.CheckpointAsync().ConfigureAwait(false);
        }

        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            PartitionContext removedContext;
            _partitions.TryRemove(context.Lease.PartitionId, out removedContext);

            return Task.FromResult(true);
        }
    }
}