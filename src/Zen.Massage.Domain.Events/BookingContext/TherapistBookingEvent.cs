using System;
using Zen.Massage.Domain.BookingBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingContext
{
    [Serializable]
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
