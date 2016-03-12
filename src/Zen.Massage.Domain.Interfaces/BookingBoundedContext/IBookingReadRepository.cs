using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface IBookingReadRepository
    {
        Task<IReadBooking> GetBooking(
            TenantId tenantId, BookingId bookingId, bool includeTherapists, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureOpenBookings(
            TenantId tenantId, DateTime cutoffDate, BookingStatus status, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForCustomer(
            TenantId tenantId, CustomerId customerId, DateTime cutoffDate, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapist(
            TenantId tenantId, TherapistId therapistId, DateTime cutoffDate, CancellationToken cancellationToken);

        Task AddBooking(
            TenantId tenantId, BookingId bookingId, CustomerId customerId, DateTime proposedTime, TimeSpan duration, CancellationToken cancellationToken);

        Task UpdateBooking(
            TenantId tenantId, BookingId bookingId, BookingStatus? status, DateTime? proposedTime, TimeSpan? duration, CancellationToken cancellationToken);

        Task AddTherapistBooking(
            TenantId tenantId, BookingId bookingId, TherapistId therapistId, BookingStatus status, DateTime proposedTime, CancellationToken cancellationToken);

        Task UpdateTherapistBooking(
            TenantId tenantId, BookingId bookingId, TherapistId therapistId, BookingStatus? status, DateTime? proposedTime, CancellationToken cancellationToken);
    }
}