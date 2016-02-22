using System;
using System.Collections.Generic;
using NanoMessageBus.Serialization;

namespace NanoMessageBus.Channels
{
    public class ServiceBusChannelGroupConfiguration : IChannelGroupConfiguration
    {
        private readonly ICollection<Type> _messageTypes = new HashSet<Type>();

        public ServiceBusChannelGroupConfiguration()
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
            DispatchTable = new ServiceBusDispatchTable();
            MessageAdapter = new ServiceBusMessageAdapter(this);
            Serializer = null;
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

        public virtual ServiceBusMessageAdapter MessageAdapter { get; private set; }

        public virtual TimeSpan ReceiveTimeout { get; private set; }

        public virtual IDependencyResolver DependencyResolver { get; private set; }

        public virtual IDispatchTable DispatchTable { get; private set; }

        public virtual ISerializer Serializer { get; private set; }

        public ServiceBusChannelGroupConfiguration WithGroupName(string groupName)
        {
            GroupName = groupName;
            return this;
        }

        public ServiceBusChannelGroupConfiguration WithReceiveTimeout(TimeSpan timeout)
        {
            ReceiveTimeout = timeout;
            return this;
        }

        public ServiceBusChannelGroupConfiguration WithInputHubPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            InputHubPath = path;
            DispatchOnly = false;
            return this;
        }

        public ServiceBusChannelGroupConfiguration WithDispatchOnly()
        {
            InputHubPath = null;
            DispatchOnly = true;
            return this;
        }

        public ServiceBusChannelGroupConfiguration WithDependencyResolver(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            DependencyResolver = resolver;
            return this;
        }

        public ServiceBusChannelGroupConfiguration WithDispatchTable(IDispatchTable dispatchTable)
        {
            if (dispatchTable == null)
            {
                throw new ArgumentNullException(nameof(dispatchTable));
            }

            DispatchTable = dispatchTable;
            return this;
        }

        public ServiceBusChannelGroupConfiguration WithChannelMessageBuilder(IChannelMessageBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            MessageBuilder = builder;
            return this;
        }

        public ServiceBusChannelGroupConfiguration WithMessageAdapter(ServiceBusMessageAdapter adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }

            MessageAdapter = adapter;
            return this;
        }

        public ServiceBusChannelGroupConfiguration WithSerializer(ISerializer serializer)
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