using System;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
    public class BookingBidEvent : BookingEvent
    {
        public BookingBidEvent(TenantId tenantId, BookingId bookingId, TherapistId therapistId, DateTime proposedTime)
            : base(tenantId, bookingId)
        {
            TherapistId = therapistId;
            ProposedTime = proposedTime;
        }

        public TherapistId TherapistId { get; private set; }

        public DateTime ProposedTime { get; private set; }
    }
}