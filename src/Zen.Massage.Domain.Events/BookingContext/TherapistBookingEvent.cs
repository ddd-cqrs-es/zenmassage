using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class TherapistBookingEvent : BookingEvent
    {
        public TherapistBookingEvent(BookingId bookingId, TherapistId therapistId)
            : base(bookingId)
        {
            TherapistId = therapistId;
        }

        public TherapistId TherapistId { get; private set; }
    }
}
