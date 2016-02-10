using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IBookingReadRepository
    {
        Task<IReadBooking> GetBooking(
            Guid bookingId, bool includeTherapists, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureOpenBookings(
            DateTime cutoffDate, BookingStatus status, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForClient(
            Guid clientId, DateTime cutoffDate, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapist(
            Guid therapistId, DateTime cutoffDate, CancellationToken cancellationToken);

        Task AddBooking(
            Guid bookingId, Guid clientId, DateTime proposedTime, TimeSpan duration, CancellationToken cancellationToken);

        Task UpdateBooking(
            Guid bookingId, BookingStatus? status, DateTime? proposedTime, TimeSpan? duration, CancellationToken cancellationToken);

        Task AddTherapistBooking(
            Guid bookingId, Guid therapistId, BookingStatus status, DateTime proposedTime, CancellationToken cancellationToken);

        Task UpdateTherapistBooking(
            Guid bookingId, Guid therapistId, BookingStatus? status, DateTime? proposedTime, CancellationToken cancellationToken);
    }
}