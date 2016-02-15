using System;

namespace Zen.Massage.Domain.BookingContext
{
    [Serializable]
    public class BookingTenderEvent : BookingEvent
    {
        public BookingTenderEvent(BookingId bookingId)
            : base(bookingId)
        {
        }
    }
}