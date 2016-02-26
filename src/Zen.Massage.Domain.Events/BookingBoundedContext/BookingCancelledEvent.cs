using System;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
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
