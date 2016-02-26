using System;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
    public class TherapistBookingConfirmedEvent : TherapistBookingEvent
    {
        public TherapistBookingConfirmedEvent(BookingId bookingId, TherapistId therapistId)
            : base(bookingId, therapistId)
        {
        }
    }
}