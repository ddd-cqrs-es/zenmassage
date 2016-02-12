using System;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class EventHubConnectionFactory
    {
        private EventHubClient _client;

        public void SetConnectionString(string connectionString)
        {
            _client = EventHubClient.CreateFromConnectionString(connectionString);
        }

        public IMessagingChannel CreateConnection()
        {
            throw new NotImplementedException();
        }
    }
}