using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class BookingCancelledEvent : BookingEvent
    {
        public BookingCancelledEvent(BookingId bookingId, string reason)
            : this(bookingId, TherapistId.Empty, reason)
        {
        }

        public BookingCancelledEvent(BookingId bookingId, TherapistId therapistId, string reason)
            : base(bookingId)
        {
            TherapistId = therapistId;
            Reason = reason;
        }

        public TherapistId TherapistId { get; private set; }

        public string Reason { get; private set; }
    }
}
