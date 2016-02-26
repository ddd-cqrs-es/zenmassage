using System;
using Zen.Massage.Domain.BookingBoundedContext;

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