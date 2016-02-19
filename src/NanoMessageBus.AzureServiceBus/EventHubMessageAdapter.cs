using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace NanoMessageBus.Channels
{
    public class EventHubMessageAdapter
    {
        private readonly EventHubChannelGroupConfiguration _configuration;

        public EventHubMessageAdapter(EventHubChannelGroupConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual ChannelMessage Build(EventData message)
        {
            return Translate(message);
        }

        public virtual EventData Build(ChannelMessage message)
        {
            return Translate(message);
        }

        protected virtual ChannelMessage Translate(EventData message)
        {
            var messageId = (Guid)message.Properties["MessageId"];
            var correlationId = (Guid)message.Properties["CorrelationId"];
            var returnAddress = (Uri)message.Properties["ReturnUri"];
            var type = (string)message.Properties["Type"];
            var contentType = (string)message.Properties["ContentType"];
            var contentEncoding = (string)message.Properties["ContentEncoding"];

            var headers = message.Properties
                .ToDictionary(prop => prop.Key, prop => (string)prop.Value);

            var payload = Deserialize(
                message.GetBodyStream(),
                type,
                contentType,
                contentEncoding);

            return new ChannelMessage(
                messageId,
                correlationId,
                returnAddress,
                headers,
                payload);
        }

        protected virtual EventData Translate(ChannelMessage message)
        {
            var serializer = _configuration.Serializer;

            var stream = new MemoryStream();
            var messages = (message.Messages ?? new object[0]).ToArray();
            if (messages.Length > 1)
            {
                serializer.Serialize(stream, messages);
            }
            else
            {
                serializer.Serialize(stream, message.Messages[0]);
            }

            var exactStreamBuffer = new ArraySegment<byte>(stream.GetBuffer(), 0, (int)stream.Length);
            var eventData = new EventData(exactStreamBuffer.ToArray());
            eventData.Properties["MessageId"] = message.MessageId;
            eventData.Properties["CorrelationId"] = message.CorrelationId;
            eventData.Properties["ReturnUri"] = message.ReturnAddress;
            eventData.Properties["Type"] = message.Messages[0].GetType().FullName;
            eventData.Properties["ContentType"] = !string.IsNullOrEmpty(serializer.ContentFormat)
                ? serializer.ContentFormat
                : "application/nanomessagebus";
            eventData.Properties["ContentEncoding"] = serializer.ContentEncoding ?? string.Empty;

            foreach (var header in message.Headers)
            {
                eventData.Properties[header.Key] = header.Value;
            }

            return eventData;
        }

        private IEnumerable<object> Deserialize(Stream body, string type, string format, string encoding)
        {
            var parsedType = Type.GetType(type, false, true) ?? typeof(object);
            var deserialized = _configuration.Serializer.Deserialize(body, parsedType, format, encoding);
            var collection = deserialized as object[];
            return collection ?? new[] { deserialized };
        }
    }
}
