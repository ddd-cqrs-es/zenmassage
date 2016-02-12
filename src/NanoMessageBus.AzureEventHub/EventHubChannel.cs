using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class EventHubChannel : IMessagingChannel
    {
        private class ChannelEventProcessorFactory : IEventProcessorFactory
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

        private class ChannelEventProcessor : IEventProcessor
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
                throw new NotImplementedException();
            }

            public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
            {
                throw new NotImplementedException();
            }

            public Task CloseAsync(PartitionContext context, CloseReason reason)
            {
                throw new NotImplementedException();
            }

            private void TryDeliver(EventData message)
            {
                try
                {
                    
                }
                catch (ChannelConnectionException)
                {
                    Log.Warn("Channel {0} has become unavailable, aborting current transaction.", this.identifier);
                    _owner.CurrentTransaction.TryDispose();
                    throw;
                }
                catch (PoisonMessageException e)
                {
                    Log.Warn("Wire message {0} on channel {1} could not be deserialized; forwarding to poison message exchange.", messageId, this.identifier);
                    this.ForwardToPoisonMessageExchange(message, e);
                }
                catch (DeadLetterException e)
                {
                    var seconds = (SystemTime.UtcNow - e.Expiration).TotalSeconds;
                    Log.Info("Wire message {0} on channel {1} expired on the wire {2:n3} seconds ago; forwarding to dead letter exchange.", messageId, this.identifier, seconds);
                    this.ForwardTo(message, this.configuration.DeadLetterExchange);
                }
                catch (Exception e)
                {
                    _owner.CurrentTransaction.TryDispose();
                    this.RetryMessage(message, e);
                }
            }
        }

        private EventHubClient _eventHubClient;
        private EventProcessorHost _eventProcessorHost;
        
        public bool Active { get; }

        public ChannelMessage CurrentMessage { get; }

        public IDependencyResolver CurrentResolver { get; }

        public IChannelTransaction CurrentTransaction { get; }

        public IChannelGroupConfiguration CurrentConfiguration { get; }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Send(ChannelEnvelope envelope)
        {
            if (_eventHubClient == null)
            {
                _eventHubClient = EventHubClient.Create(eventHubPath);
            }

            var eventData = new EventData();
            eventData.
            envelope.Message.ActiveMessage
            _eventHubClient.
        }

        public async void Receive(Action<IDeliveryContext> callback)
        {
            _eventProcessorHost = new EventProcessorHost(
                eventHubPath, consumerGroupName, hubConnectionString, storageConnectionString);
            await _eventProcessorHost.RegisterEventProcessorFactoryAsync(new ChannelEventProcessorFactory(this, callback)).ConfigureAwait(false);
        }

        public IDispatchContext PrepareDispatch(object message = null, IMessagingChannel channel = null)
        {
            throw new NotImplementedException();
        }

        public void BeginShutdown()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}