using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
            return Translate(message);
        }

        public virtual BrokeredMessage[] Build(ChannelMessage message)
        {
            return Translate(message);
        }

        protected virtual ChannelMessage Translate(BrokeredMessage message)
        {
            var messageId = (Guid)message.Properties["MessageId"];
            var correlationId = (Guid)message.Properties["CorrelationId"];
            var returnAddress = (Uri)message.Properties["ReturnUri"];
            var type = (string)message.Properties["Type"];
            var contentType = (string)message.Properties["ContentType"];
            var contentEncoding = (string)message.Properties["ContentEncoding"];

            var headers = message.Properties
                .ToDictionary(prop => prop.Key, prop => (string)prop.Value);

            var payload = GetMessageBody(message);

            return new ChannelMessage(
                messageId,
                correlationId,
                returnAddress,
                headers,
                new [] { payload });
        }

        // TODO: Make concurrent
        private static readonly Dictionary<string, Func<BrokeredMessage, object>> 
            TypeMapper = new Dictionary<string, Func<BrokeredMessage, object>>();
        private static object GetMessageBody(BrokeredMessage message)
        {
            var fullTypeName = (string)message.Properties["Type"];
            if (string.IsNullOrEmpty(fullTypeName))
            {
                throw new InvalidOperationException("Unable to determine serialized message body type.");
            }

            Func<BrokeredMessage, object> mapper;
            if (!TypeMapper.TryGetValue(fullTypeName, out mapper))
            {
                Type type;
                int index = fullTypeName.IndexOf(',');
                var typeName = (index > -1) ? fullTypeName.Substring(0, index) : fullTypeName;
                var assemblyName = (index > -1) ? fullTypeName.Substring(index + 1) : string.Empty;
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    type = Assembly.Load(assemblyName).GetType(typeName);
                }
                else
                {
                    type = Type.GetType(typeName);
                }

                var mi = typeof(BrokeredMessage)
                    .GetMethod("GetBody")
                    .MakeGenericMethod(type);
                mapper = (bm) => mi.Invoke(bm, null);
                TypeMapper.Add(fullTypeName, mapper);
            }

            return mapper(message);
        }

        protected virtual BrokeredMessage[] Translate(ChannelMessage message)
        {
            var messages = new List<BrokeredMessage>();
            foreach (var payload in message.Messages)
            {
                var brokeredMessage = new BrokeredMessage(payload);
                brokeredMessage.Properties["MessageId"] = message.MessageId;
                brokeredMessage.Properties["CorrelationId"] = message.CorrelationId;
                brokeredMessage.Properties["ReturnUri"] = message.ReturnAddress;
                brokeredMessage.Properties["Type"] = $"{payload.GetType().FullName}, {payload.GetType().Assembly.FullName}";

                foreach (var header in message.Headers)
                {
                    brokeredMessage.Properties[header.Key] = header.Value;
                }

                messages.Add(brokeredMessage);
            }

            return messages.ToArray();
        }
    }
}
