using System;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
    public class TherapistBookingAcceptedEvent : TherapistBookingEvent
    {
        public TherapistBookingAcceptedEvent(BookingId bookingId, TherapistId therapistId)
            : base(bookingId, therapistId)
        {
        }
    }
}