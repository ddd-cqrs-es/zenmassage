using System;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
    public class BookingBidEvent : BookingEvent
    {
        public BookingBidEvent(BookingId bookingId, TherapistId therapistId, DateTime proposedTime)
            : base(bookingId)
        {
            TherapistId = therapistId;
            ProposedTime = proposedTime;
        }

        public TherapistId TherapistId { get; private set; }

        public DateTime ProposedTime { get; private set; }
    }
}