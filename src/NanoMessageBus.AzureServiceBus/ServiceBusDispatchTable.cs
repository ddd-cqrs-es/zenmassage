using System;
using System.Collections.Generic;

namespace NanoMessageBus.Channels
{
    public class ServiceBusDispatchTable : IDispatchTable
    {
        public ICollection<Uri> this[Type messageType]
        {
            get
            {
                if (messageType == null)
                {
                    throw new ArgumentNullException(nameof(messageType));
                }

                return new[] { new Uri("fanout://" + messageType.FullName, UriKind.Absolute) };
            }
        }

        public void AddSubscriber(Uri subscriber, Type messageType, DateTime expiration)
        {
        }

        public void AddRecipient(Uri recipient, Type messageType)
        {
        }

        public void Remove(Uri address, Type messageType)
        {
        }
    }
}