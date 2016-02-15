using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class TherapistBookingAcceptedEvent : TherapistBookingEvent
    {
        public TherapistBookingAcceptedEvent(BookingId bookingId, TherapistId therapistId)
            : base(bookingId, therapistId)
        {
        }
    }
}