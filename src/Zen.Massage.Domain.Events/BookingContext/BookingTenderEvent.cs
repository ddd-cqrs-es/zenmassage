using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class BookingTenderEvent : BookingEvent
    {
        public BookingTenderEvent(BookingId bookingId)
            : base(bookingId)
        {
        }
    }
}