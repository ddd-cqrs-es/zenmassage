using System;
using Microsoft.Data.Edm.Validation;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class AzureTopicConnectionFactory
    {
        private string _connectionString;
        private NamespaceManager _namespaceManager;

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            _namespaceManager = NamespaceManager.CreateFromConnectionString(_connectionString);
        }

        public IMessagingChannel CreateConnection(
            ServiceBusConnector connector,
            ServiceBusChannelGroupConfiguration config)
        {
            connector.CurrentState = ConnectionState.Opening;

            TopicDescription topicDescription;
            if (!_namespaceManager.TopicExists(config.InputHubPath))
            {
                topicDescription = _namespaceManager.CreateTopic(config.InputHubPath);
            }
            else
            {
                topicDescription = _namespaceManager.GetTopic(config.InputHubPath);
            }

            SubscriptionClient subscriptionClient = null;
            if (!config.DispatchOnly)
            {
                subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                    _connectionString, config.InputHubPath, "All Messages", ReceiveMode.PeekLock);
            }

            var topicClient = TopicClient.CreateFromConnectionString(_connectionString, config.InputHubPath);
            return new AzureTopicChannel(connector, config, topicDescription, topicClient, subscriptionClient);
        }
    }
}