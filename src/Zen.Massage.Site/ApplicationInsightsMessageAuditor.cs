using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using NanoMessageBus;
using NanoMessageBus.Channels;

namespace Zen.Massage.Site
{
    public class ApplicationInsightsMessageAuditor : IMessageAuditor
    {
        private readonly IMessagingChannel _channel;
        private readonly TelemetryClient _telemetryClient;

        public ApplicationInsightsMessageAuditor(IMessagingChannel channel)
        {
            _channel = channel;
            _telemetryClient = new TelemetryClient();
        }

        public void Dispose()
        {
        }

        public void AuditReceive(IDeliveryContext delivery)
        {
            _telemetryClient.TrackEvent("ReceiveMessage",
                new Dictionary<string, string>
                {
                    { "ChannelGroup", _channel.CurrentConfiguration.GroupName },
                    { "MessageType", delivery.CurrentMessage.ActiveMessage.GetType().Name }
                });
        }

        public void AuditSend(ChannelEnvelope envelope, IDeliveryContext delivery)
        {
            _telemetryClient.TrackEvent("SendMessage",
                new Dictionary<string, string>
                {
                    { "ChannelGroup", _channel.CurrentConfiguration.GroupName },
                    { "MessageType", envelope.Message.ActiveMessage.GetType().Name }
                });
        }
    }
}