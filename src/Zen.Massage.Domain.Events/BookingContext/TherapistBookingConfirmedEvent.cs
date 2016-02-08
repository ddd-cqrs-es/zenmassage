using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class TherapistBookingConfirmedEvent : TherapistBookingEvent
    {
        public TherapistBookingConfirmedEvent(Guid bookingId, Guid therapistId)
            : base(bookingId, therapistId)
        {
        }
    }
}