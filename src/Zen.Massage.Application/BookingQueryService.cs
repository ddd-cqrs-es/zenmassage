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
