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
            var messageId = (Guid)message.SystemProperties["MessageId"];
            var correlationId = (Guid)message.SystemProperties["CorrelationId"];
            var returnAddress = (Uri)message.SystemProperties["ReturnUri"];
            var type = (string)message.SystemProperties["Type"];
            var contentType = (string)message.SystemProperties["ContentType"];
            var contentEncoding = (string)message.SystemProperties["ContentEncoding"];

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
            eventData.SystemProperties["MessageId"] = message.MessageId;
            eventData.SystemProperties["CorrelationId"] = message.CorrelationId;
            eventData.SystemProperties["ReturnUri"] = message.ReturnAddress;
            eventData.SystemProperties["Type"] = message.Messages[0].GetType().FullName;
            eventData.SystemProperties["ContentType"] = !string.IsNullOrEmpty(serializer.ContentFormat)
                ? serializer.ContentFormat
                : "application/nanomessagebus";
            eventData.SystemProperties["ContentEncoding"] = serializer.ContentEncoding ?? string.Empty;

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
