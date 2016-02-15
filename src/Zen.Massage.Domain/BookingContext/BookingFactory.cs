using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class BookingFactory : IBookingFactory
    {
        public IBooking Create(Guid clientId, DateTime proposedTime, TimeSpan duration)
        {
            var booking = new BookingAggregate();
            booking.Create(clientId, proposedTime, duration);
            return booking;
        }
    }
}
