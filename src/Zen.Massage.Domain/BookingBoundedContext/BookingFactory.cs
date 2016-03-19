using System;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public class BookingFactory : IBookingFactory
    {
        public IBooking Create(TenantId tenantId,CustomerId customerId, TherapyId therapyId, DateTimeOffset proposedTime, TimeSpan duration)
        {
            var booking = new BookingAggregate();
            booking.Create(tenantId, customerId, therapyId, proposedTime, duration);
            return booking;
        }
    }
}
