using System;
using System.Collections.Generic;
using System.Linq;
using NanoMessageBus;
using NEventStore;

namespace Zen.Massage.Site
{
    public class PipelineDispatcherHook : IPipelineHook
    {
        private readonly IMessagingHost _messagingHost;
        private IChannelGroup _channelGroup;
        private IMessagingChannel _messageChannel;

        public PipelineDispatcherHook(
            IMessagingHost messagingHost)
        {
            _messagingHost = messagingHost;
            _channelGroup = _messagingHost.Initialize();
            _messageChannel = _channelGroup.OpenChannel();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public ICommit Select(ICommit committed)
        {
            return committed;
        }

        public bool PreCommit(CommitAttempt attempt)
        {
            return true;
        }

        public void PostCommit(ICommit committed)
        {
            var messageId = committed.CommitId;
            var correlationId = Guid.NewGuid();
            var message = new ChannelMessage(
                messageId,
                correlationId,
                new Uri("uri://noreply.zenmassage.com"),
                new Dictionary<string, string>(),
                committed.Events.Select(e => e.Body));
            var envelope = new ChannelEnvelope(
                message,
                new[]
                {
                    new Uri("uri://domain.zenmassage.com"),  
                });
            _messageChannel.Send(envelope);
        }

        public void OnPurge(string bucketId)
        {
        }

        public void OnDeleteStream(string bucketId, string streamId)
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_messageChannel != null)
                {
                    _messageChannel.Dispose();
                    _messageChannel = null;
                }
                if (_channelGroup != null)
                {
                    _channelGroup.Dispose();
                    _channelGroup = null;
                }
            }
        }
    }
}