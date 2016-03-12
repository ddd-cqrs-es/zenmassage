using System;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public class BookingFactory : IBookingFactory
    {
        public IBooking Create(TenantId tenantId,CustomerId customerId, DateTime proposedTime, TimeSpan duration)
        {
            var booking = new BookingAggregate();
            booking.Create(tenantId, customerId, proposedTime, duration);
            return booking;
        }
    }
}
