using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Threading.Tasks;
using Zen.Infrastructure.ReadRepository.DataAccess;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Infrastructure.ReadRepository
{
    public class BookingReadRepository : IBookingReadRepository
    {
        public async Task<IReadBooking> GetBooking(Guid bookingId, bool includeTherapists)
        {
            using (var context = new BookingEntityContext())
            {
                // Determine depth of information we will be returning
                DbQuery<DbBooking> query = context.Bookings;
                if (includeTherapists)
                {
                    query = query.Include("TherapistBookings");
                }

                // Issue async query
                var result = await query
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId)
                    .ConfigureAwait(false);
                if (result == null)
                {
                    return null;
                }

                // Returned booking wrapped in helper
                return new ReadBooking(result);
            }
        }

        public Task<IEnumerable<IReadBooking>> GetFutureOpenBookings(DateTime cutoffDate, BookingStatus status)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IReadBooking>> GetFutureBookingsForClient(Guid clientId, DateTime cutoffDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapist(Guid therapistId, DateTime cutoffDate)
        {
            throw new NotImplementedException();
        }

        public Task AddBooking(Guid bookingId, Guid clientId, DateTime proposedTime, TimeSpan duration)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBooking(Guid bookingId, BookingStatus status, DateTime proposedTime, TimeSpan duration)
        {
            throw new NotImplementedException();
        }
    }
}
