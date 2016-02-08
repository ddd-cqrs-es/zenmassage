using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class BookingCancelledEvent : BookingEvent
    {
        public BookingCancelledEvent(Guid bookingId, string reason)
            : this(bookingId, Guid.Empty, reason)
        {
        }

        public BookingCancelledEvent(Guid bookingId, Guid therapistId, string reason)
            : base(bookingId)
        {
            TherapistId = therapistId;
            Reason = reason;
        }

        public Guid TherapistId { get; private set; }

        public string Reason { get; private set; }
    }
}
