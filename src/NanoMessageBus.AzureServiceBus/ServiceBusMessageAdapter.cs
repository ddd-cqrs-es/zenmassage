using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class ServiceBusMessageAdapter
    {
        private readonly ServiceBusChannelGroupConfiguration _configuration;

        public ServiceBusMessageAdapter(ServiceBusChannelGroupConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual ChannelMessage Build(BrokeredMessage message)
        {
            var messageId = (Guid)message.Properties["MessageId"];
            var correlationId = (Guid)message.Properties["CorrelationId"];
            var returnAddress = (Uri)message.Properties["ReturnUri"];
            var headers = message.Properties
                .ToDictionary(prop => prop.Key, prop => (string)prop.Value);
            return new ChannelMessage(
                messageId,
                correlationId,
                returnAddress,
                headers,
                message.GetBody<IList<object>>());
        }

        public virtual BrokeredMessage Build(ChannelMessage message)
        {
            var brokeredMessage = new BrokeredMessage(message.Messages);
            brokeredMessage.Properties["MessageId"] = message.MessageId;
            brokeredMessage.Properties["CorrelationId"] = message.CorrelationId;
            brokeredMessage.Properties["ReturnUri"] = message.ReturnAddress;
            foreach (var header in message.Headers)
            {
                brokeredMessage.Properties.Add(header.Key, header.Value);
            }
            return brokeredMessage;
        }
    }
}
