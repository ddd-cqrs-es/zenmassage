using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface IBookingReadRepository
    {
        Task<IReadBooking> GetBooking(
            BookingId bookingId, bool includeTherapists, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureOpenBookings(
            DateTime cutoffDate, BookingStatus status, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForCustomer(
            CustomerId customerId, DateTime cutoffDate, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapist(
            TherapistId therapistId, DateTime cutoffDate, CancellationToken cancellationToken);

        Task AddBooking(
            BookingId bookingId, CustomerId customerId, DateTime proposedTime, TimeSpan duration, CancellationToken cancellationToken);

        Task UpdateBooking(
            BookingId bookingId, BookingStatus? status, DateTime? proposedTime, TimeSpan? duration, CancellationToken cancellationToken);

        Task AddTherapistBooking(
            BookingId bookingId, TherapistId therapistId, BookingStatus status, DateTime proposedTime, CancellationToken cancellationToken);

        Task UpdateTherapistBooking(
            BookingId bookingId, TherapistId therapistId, BookingStatus? status, DateTime? proposedTime, CancellationToken cancellationToken);
    }
}