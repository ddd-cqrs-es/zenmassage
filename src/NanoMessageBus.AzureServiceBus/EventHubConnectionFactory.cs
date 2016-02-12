using System;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class EventHubConnectionFactory
    {
        private string _hubConnectionString;
        private string _storeConnectionString;

        public void SetHubConnectionString(string hubConnectionString)
        {
            _hubConnectionString = hubConnectionString;
        }

        public void SetStoreConnectionString(string storeConnectionString)
        {
            _storeConnectionString = storeConnectionString;
        }

        public IMessagingChannel CreateConnection()
        {
            throw new NotImplementedException();
        }
    }
}