using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class BookingBidEvent : BookingEvent
    {
        public BookingBidEvent(Guid bookingId, Guid therapistId, DateTime proposedTime)
            : base(bookingId)
        {
            TherapistId = therapistId;
            ProposedTime = proposedTime;
        }

        public Guid TherapistId { get; private set; }

        public DateTime ProposedTime { get; private set; }
    }
}