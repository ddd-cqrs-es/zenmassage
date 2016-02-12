using System;
using System.Collections.Generic;
using NanoMessageBus.Serialization;

namespace NanoMessageBus.Channels
{
    public class EventHubChannelGroupConfiguration : IChannelGroupConfiguration
    {
        private readonly ICollection<Type> _messageTypes = new HashSet<Type>();

        public EventHubChannelGroupConfiguration()
        {
            InputHubPath = null;
            GroupName = null;
            DispatchOnly = true;
            Synchronous = false;
            MaxDispatchBuffer = 1;
            MinWorkers = 1;
            MaxWorkers = 1;
            ReturnAddress = null;
            MessageBuilder = new DefaultChannelMessageBuilder();
            ReceiveTimeout = TimeSpan.FromMilliseconds(1500);
            DependencyResolver = null;
            DispatchTable = new EventHubDispatchTable();
            Serializer = new BinarySerializer();
            MessageAdapter = new EventHubMessageAdapter(this);
        }

        public virtual string InputHubPath { get; private set; }

        public virtual string GroupName { get; private set; }

        public virtual bool Synchronous { get; private set; }

        public virtual bool DispatchOnly { get; private set; }

        public virtual int MaxDispatchBuffer { get; private set; }

        public virtual int MinWorkers { get; private set; }

        public virtual int MaxWorkers { get; private set; }

        public virtual Uri ReturnAddress { get; private set; }

        public virtual IChannelMessageBuilder MessageBuilder { get; private set; }

        public virtual EventHubMessageAdapter MessageAdapter { get; private set; }

        public virtual TimeSpan ReceiveTimeout { get; private set; }

        public virtual IDependencyResolver DependencyResolver { get; private set; }

        public virtual IDispatchTable DispatchTable { get; private set; }

        public virtual ISerializer Serializer { get; private set; }

        public EventHubChannelGroupConfiguration WithGroupName(string groupName)
        {
            GroupName = groupName;
            return this;
        }

        public EventHubChannelGroupConfiguration WithReceiveTimeout(TimeSpan timeout)
        {
            ReceiveTimeout = timeout;
            return this;
        }

        public EventHubChannelGroupConfiguration WithInputHubPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            InputHubPath = path;
            DispatchOnly = false;
            return this;
        }

        public EventHubChannelGroupConfiguration WithDispatchOnly()
        {
            InputHubPath = null;
            DispatchOnly = true;
            return this;
        }

        public EventHubChannelGroupConfiguration WithDependencyResolver(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            DependencyResolver = resolver;
            return this;
        }

        public EventHubChannelGroupConfiguration WithDispatchTable(IDispatchTable dispatchTable)
        {
            if (dispatchTable == null)
            {
                throw new ArgumentNullException(nameof(dispatchTable));
            }

            DispatchTable = dispatchTable;
            return this;
        }

        public EventHubChannelGroupConfiguration WithChannelMessageBuilder(IChannelMessageBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            MessageBuilder = builder;
            return this;
        }

        public EventHubChannelGroupConfiguration WithMessageAdapter(EventHubMessageAdapter adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }

            MessageAdapter = adapter;
            return this;
        }

        public EventHubChannelGroupConfiguration WithSerializer(ISerializer serializer)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            Serializer = serializer;
            return this;
        }
    }
}