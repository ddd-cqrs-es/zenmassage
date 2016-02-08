using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class TherapistBookingEvent : BookingEvent
    {
        public TherapistBookingEvent(Guid bookingId, Guid therapistId)
            : base(bookingId)
        {
            TherapistId = therapistId;
        }

        public Guid TherapistId { get; private set; }
    }
}
