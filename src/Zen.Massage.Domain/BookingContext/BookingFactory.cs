using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zen.Massage.Domain.BookingContext
{
    public class BookingFactory : IBookingFactory
    {
        public IBooking Create(Guid clientId, DateTime proposedTime, TimeSpan duration)
        {
            var booking = new BookingAggregate();
            booking.Initialize(
                new []
                {
                    new BookingCreatedEvent(Guid.NewGuid(), clientId, proposedTime, duration), 
                });
            return booking;
        }
    }
}
