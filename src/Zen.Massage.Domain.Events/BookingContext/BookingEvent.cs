using System;
using Zen.Massage.Domain.BookingBoundedContext;

namespace Zen.Massage.Domain.BookingContext
{
    [Serializable]
    public class BookingEvent
    {
        public BookingEvent(BookingId bookingId)
        {
            BookingId = bookingId;
        }

        public BookingId BookingId { get; private set; }
    }
}