using System;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IBookingFactory
    {
        IBooking Create(Guid clientId, DateTime proposedTime, TimeSpan duration);
    }
}
