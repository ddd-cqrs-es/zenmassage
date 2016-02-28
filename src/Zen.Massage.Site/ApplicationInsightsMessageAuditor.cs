using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using NanoMessageBus;
using NanoMessageBus.Channels;
using NanoMessageBus.Logging;

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

    public class ApplicationInsightsMessagingLogger : ILog
    {
        private readonly ILog _innerLogger;
        private readonly TelemetryClient _telemetryClient = new TelemetryClient();

        public ApplicationInsightsMessagingLogger(ILog innerLogger = null)
        {
            _innerLogger = innerLogger;
        }

        public void Verbose(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(string.Format(message, values), SeverityLevel.Verbose);
            _innerLogger?.Verbose(message, values);
        }

        public void Debug(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(string.Format(message, values), SeverityLevel.Information);
            _innerLogger?.Debug(message, values);
        }

        public void Info(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(string.Format(message, values), SeverityLevel.Information);
            _innerLogger?.Info(message, values);
        }

        public void Warn(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(string.Format(message, values), SeverityLevel.Warning);
            _innerLogger?.Warn(message, values);
        }

        public void Error(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(string.Format(message, values), SeverityLevel.Error);
            _innerLogger?.Error(message, values);
        }

        public void Fatal(string message, params object[] values)
        {
            _telemetryClient.TrackTrace(string.Format(message, values), SeverityLevel.Critical);
            _innerLogger?.Fatal(message, values);
        }

        public void Fatal(string message, Exception exception)
        {
            _telemetryClient.TrackTrace(message, SeverityLevel.Critical);
            _telemetryClient.TrackException(exception);
            _innerLogger?.Fatal(message, exception);
        }

        public void Error(string message, Exception exception)
        {
            _telemetryClient.TrackTrace(message, SeverityLevel.Error);
            _telemetryClient.TrackException(exception);
            _innerLogger?.Error(message, exception);
        }

        public void Warn(string message, Exception exception)
        {
            _telemetryClient.TrackTrace(message, SeverityLevel.Warning);
            _telemetryClient.TrackException(exception);
            _innerLogger?.Warn(message, exception);
        }

        public void Info(string message, Exception exception)
        {
            _telemetryClient.TrackTrace(message, SeverityLevel.Information);
            _telemetryClient.TrackException(exception);
            _innerLogger?.Info(message, exception);
        }

        public void Debug(string message, Exception exception)
        {
            _telemetryClient.TrackTrace(message, SeverityLevel.Information);
            _telemetryClient.TrackException(exception);
            _innerLogger?.Debug(message, exception);
        }

        public void Verbose(string message, Exception exception)
        {
            _telemetryClient.TrackTrace(message, SeverityLevel.Verbose);
            _telemetryClient.TrackException(exception);
            _innerLogger?.Verbose(message, exception);
        }
    }
}