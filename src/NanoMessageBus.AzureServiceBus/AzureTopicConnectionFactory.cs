using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class AzureTopicConnectionFactory
    {
        private string _connectionString;

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IMessagingChannel CreateConnection(
            ServiceBusConnector connector,
            ServiceBusChannelGroupConfiguration config)
        {
            connector.CurrentState = ConnectionState.Opening;

            SubscriptionClient subscriptionClient = null;
            if (!config.DispatchOnly)
            {
                subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                    _connectionString, config.InputHubPath, "allmessages");
            }

            var topicClient = TopicClient.CreateFromConnectionString(_connectionString, config.InputHubPath);
            return new AzureTopicChannel(connector, config, topicClient, subscriptionClient);
        }
    }
}