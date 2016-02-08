using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class BookingEvent
    {
        public BookingEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }

        public Guid BookingId { get; private set; }
    }
}