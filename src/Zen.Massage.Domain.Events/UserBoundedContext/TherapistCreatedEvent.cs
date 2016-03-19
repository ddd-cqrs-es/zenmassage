using System;

namespace Zen.Massage.Domain.UserBoundedContext
{
    [Serializable]
    public class TherapistCreatedEvent : TherapistEvent
    {
        public TherapistCreatedEvent(TherapistId therapistId)
            : base(therapistId)
        {
        }
    }
}