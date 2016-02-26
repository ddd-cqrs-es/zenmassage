using System;

namespace Zen.Massage.Domain.BookingBoundedContext
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