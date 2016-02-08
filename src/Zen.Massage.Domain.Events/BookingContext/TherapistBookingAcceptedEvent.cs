using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class TherapistBookingAcceptedEvent : TherapistBookingEvent
    {
        public TherapistBookingAcceptedEvent(Guid bookingId, Guid therapistId)
            : base(bookingId, therapistId)
        {
        }
    }
}