using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IBookingReadRepository
    {
        Task<IReadBooking> GetBooking(Guid bookingId, bool includeTherapists);

        Task<IEnumerable<IReadBooking>> GetFutureOpenBookings(
            DateTime cutoffDate, BookingStatus status);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForClient(
            Guid clientId, DateTime cutoffDate);

        Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapist(
            Guid therapistId, DateTime cutoffDate);

        Task AddBooking(Guid bookingId, Guid clientId, DateTime proposedTime, TimeSpan duration);

        Task UpdateBooking(Guid bookingId, BookingStatus status, DateTime proposedTime, TimeSpan duration);
    }
}