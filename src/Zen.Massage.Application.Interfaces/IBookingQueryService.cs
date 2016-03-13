using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zen.Massage.Domain.BookingBoundedContext;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Application
{
    public interface IBookingQueryService
    {
        Task<IReadBooking> GetBookingAsync(
            TenantId tenantId,
            BookingId bookingId,
            bool includeTherapists,
            CancellationToken cancellationToken);

        Task<IReadBooking> GetBookingScopedForUserAsync(
            Guid userId,
            TenantId tenantId,
            BookingId bookingId,
            CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureOpenBookingsAsync(
            TenantId tenantId,
            DateTime cutoffDate,
            BookingStatus status,
            CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForCustomerAsync(
            TenantId tenantId,
            CustomerId customerId,
            DateTime cutoffDate,
            CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapistAsync(
            TenantId tenantId,
            TherapistId therapistId,
            DateTime cutoffDate,
            CancellationToken cancellationToken);
    }
}
