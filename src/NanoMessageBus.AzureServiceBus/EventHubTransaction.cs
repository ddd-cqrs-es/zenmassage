using System;

namespace NanoMessageBus.Channels
{
    public class EventHubTransaction : IChannelTransaction
    {
        public bool Finished { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Register(Action callback)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}