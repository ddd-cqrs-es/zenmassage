﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface IBookingReadRepository
    {
        Task<IReadBooking> GetBookingAsync(
            TenantId tenantId, BookingId bookingId, bool includeTherapists, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureOpenBookingsAsync(
            TenantId tenantId, DateTime cutoffDate, BookingStatus status, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForCustomerAsync(
            TenantId tenantId, CustomerId customerId, DateTime cutoffDate, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapistAsync(
            TenantId tenantId, TherapistId therapistId, DateTime cutoffDate, CancellationToken cancellationToken);

        Task AddBookingAsync(
            TenantId tenantId, BookingId bookingId, CustomerId customerId, DateTime proposedTime, TimeSpan duration, CancellationToken cancellationToken);

        Task UpdateBookingAsync(
            TenantId tenantId, BookingId bookingId, BookingStatus? status, DateTime? proposedTime, TimeSpan? duration, CancellationToken cancellationToken);

        Task AddTherapistBookingAsync(
            TenantId tenantId, BookingId bookingId, TherapistId therapistId, BookingStatus status, DateTime proposedTime, CancellationToken cancellationToken);

        Task UpdateTherapistBookingAsync(
            TenantId tenantId, BookingId bookingId, TherapistId therapistId, BookingStatus? status, DateTime? proposedTime, CancellationToken cancellationToken);
    }
}