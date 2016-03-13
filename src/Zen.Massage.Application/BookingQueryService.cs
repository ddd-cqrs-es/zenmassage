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
    public class BookingQueryService : IBookingQueryService
    {
        private readonly IBookingReadRepository _bookingReadRepository;

        public BookingQueryService(IBookingReadRepository bookingReadRepository)
        {
            _bookingReadRepository = bookingReadRepository;
        }

        public Task<IReadBooking> GetBookingAsync(
            TenantId tenantId,
            BookingId bookingId,
            bool includeTherapists,
            CancellationToken cancellationToken)
        {
            return _bookingReadRepository
                .GetBookingAsync(
                    tenantId,
                    bookingId,
                    includeTherapists,
                    cancellationToken);
        }

        public async Task<IReadBooking> GetBookingScopedForUserAsync(
            Guid userId,
            TenantId tenantId,
            BookingId bookingId,
            CancellationToken cancellationToken)
        {
            // Get the associated booking
            var booking = await _bookingReadRepository
                .GetBookingAsync(tenantId, bookingId, true, cancellationToken)
                .ConfigureAwait(true);
            if (booking == null)
            {
                // Booking not found
                return null;
            }

            // Associated user must be customer or therapist
            if (booking.CustomerId.Id != userId)
            {
                // Limit information returned to only therapist data
                booking = booking.LimitToTherapist(new TherapistId(userId));
                if (!booking.TherapistBookings.Any())
                {
                    // Booking is not known to the current caller as customer or therapist
                    return null;
                }
            }

            return booking;
        }

        public Task<IEnumerable<IReadBooking>> GetFutureOpenBookingsAsync(
            TenantId tenantId,
            DateTime cutoffDate,
            BookingStatus status,
            CancellationToken cancellationToken)
        {
            return _bookingReadRepository
                .GetFutureOpenBookingsAsync(
                    tenantId,
                    cutoffDate,
                    status,
                    cancellationToken);
        }

        public Task<IEnumerable<IReadBooking>> GetFutureBookingsForCustomerAsync(
            TenantId tenantId,
            CustomerId customerId,
            DateTime cutoffDate,
            CancellationToken cancellationToken)
        {
            return _bookingReadRepository
                .GetFutureBookingsForCustomerAsync(
                    tenantId,
                    customerId,
                    cutoffDate,
                    cancellationToken);
        }

        public Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapistAsync(
            TenantId tenantId, TherapistId therapistId, DateTime cutoffDate, CancellationToken cancellationToken)
        {
            return _bookingReadRepository
                .GetFutureBookingsForTherapistAsync(
                    tenantId,
                    therapistId,
                    cutoffDate,
                    cancellationToken);
        }
    }
}
