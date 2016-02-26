using System;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface IBookingFactory
    {
        IBooking Create(CustomerId customerId, DateTime proposedTime, TimeSpan duration);
    }
}
