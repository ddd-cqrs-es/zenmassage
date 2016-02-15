using System;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IBookingFactory
    {
        IBooking Create(ClientId clientId, DateTime proposedTime, TimeSpan duration);
    }
}
