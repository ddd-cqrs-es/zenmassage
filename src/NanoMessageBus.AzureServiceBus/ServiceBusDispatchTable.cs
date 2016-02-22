using System;
using System.Collections.Generic;

namespace NanoMessageBus.Channels
{
    public class ServiceBusDispatchTable : IDispatchTable
    {
        public ICollection<Uri> this[Type messageType]
        {
            get { throw new NotImplementedException(); }
        }

        public void AddSubscriber(Uri subscriber, Type messageType, DateTime expiration)
        {
            throw new NotImplementedException();
        }

        public void AddRecipient(Uri recipient, Type messageType)
        {
            throw new NotImplementedException();
        }

        public void Remove(Uri address, Type messageType)
        {
            throw new NotImplementedException();
        }
    }
}