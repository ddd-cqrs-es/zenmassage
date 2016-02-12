using System;

namespace NanoMessageBus.Channels
{
    public class EventHubTransaction : IChannelTransaction
    {
        public bool Finished { get; private set; }

        public void Dispose()
        {
        }

        public void Register(Action callback)
        {
        }

        public void Commit()
        {
        }

        public void Rollback()
        {
        }
    }
}