using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IBookingReadRepository
    {
        Task<IReadBooking> GetBooking(
            BookingId bookingId, bool includeTherapists, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureOpenBookings(
            DateTime cutoffDate, BookingStatus status, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForClient(
            ClientId clientId, DateTime cutoffDate, CancellationToken cancellationToken);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapist(
            TherapistId therapistId, DateTime cutoffDate, CancellationToken cancellationToken);

        Task AddBooking(
            BookingId bookingId, ClientId clientId, DateTime proposedTime, TimeSpan duration, CancellationToken cancellationToken);

        Task UpdateBooking(
            BookingId bookingId, BookingStatus? status, DateTime? proposedTime, TimeSpan? duration, CancellationToken cancellationToken);

        Task AddTherapistBooking(
            BookingId bookingId, TherapistId therapistId, BookingStatus status, DateTime proposedTime, CancellationToken cancellationToken);

        Task UpdateTherapistBooking(
            BookingId bookingId, TherapistId therapistId, BookingStatus? status, DateTime? proposedTime, CancellationToken cancellationToken);
    }
}