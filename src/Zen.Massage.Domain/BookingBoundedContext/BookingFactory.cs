using System;
using Zen.Massage.Domain.BookingContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public class BookingFactory : IBookingFactory
    {
        public IBooking Create(CustomerId customerId, DateTime proposedTime, TimeSpan duration)
        {
            var booking = new BookingAggregate();
            booking.Create(customerId, proposedTime, duration);
            return booking;
        }
    }
}
