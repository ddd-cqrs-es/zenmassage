using System;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface IBookingFactory
    {
        IBooking Create(TenantId tenantId, CustomerId customerId, DateTime proposedTime, TimeSpan duration);
    }
}
