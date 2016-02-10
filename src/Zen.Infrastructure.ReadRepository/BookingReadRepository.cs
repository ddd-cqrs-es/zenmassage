using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zen.Infrastructure.ReadRepository.DataAccess;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Infrastructure.ReadRepository
{
    public class BookingReadRepository : IBookingReadRepository
    {
        public async Task<IReadBooking> GetBooking(Guid bookingId, bool includeTherapists, CancellationToken cancellationToken)
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
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId, cancellationToken)
                    .ConfigureAwait(false);
                if (result == null)
                {
                    return null;
                }

                // Returned booking wrapped in helper
                return new ReadBooking(result);
            }
        }

        public async Task<IEnumerable<IReadBooking>> GetFutureOpenBookings(DateTime cutoffDate, BookingStatus status, CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var query = await context.Bookings
                    .Include("TherapistBookings")
                    .Where(b =>
                        b.ProposedTime > cutoffDate &&
                        b.Status != BookingStatus.Provisional &&
                        b.Status != BookingStatus.CancelledByClient)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
                return query
                    .Select(b => new ReadBooking(b));
            }
        }

        public async Task<IEnumerable<IReadBooking>> GetFutureBookingsForClient(Guid clientId, DateTime cutoffDate, CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var query = await context.Bookings
                    .Include("TherapistBookings")
                    .Where(b =>
                        b.ProposedTime > cutoffDate &&
                        b.ClientId == clientId &&
                        b.Status != BookingStatus.Provisional &&
                        b.Status != BookingStatus.CancelledByClient)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
                return query
                    .Select(b => new ReadBooking(b));
            }
        }

        public async Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapist(Guid therapistId, DateTime cutoffDate, CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var query = await context.TherapistBookings
                    .Include("Booking")
                    .Where(tb =>
                        tb.Booking.ProposedTime > cutoffDate &&
                        tb.TherapistId == therapistId &&
                        tb.Booking.Status != BookingStatus.Provisional &&
                        tb.Booking.Status != BookingStatus.CancelledByClient)
                    .Select(tb => tb.Booking)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
                return query
                    .Select(b => new ReadBooking(b));
            }
        }

        public async Task AddBooking(Guid bookingId, Guid clientId, DateTime proposedTime, TimeSpan duration, CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var booking =
                    new DbBooking
                    {
                        BookingId = (bookingId != Guid.Empty) ? bookingId : Guid.NewGuid(),
                        Status = BookingStatus.Provisional,
                        ClientId = clientId,
                        ProposedTime = proposedTime,
                        Duration = duration
                    };
                context.Bookings.Add(booking);
                await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task UpdateBooking(Guid bookingId, BookingStatus status, DateTime proposedTime, TimeSpan duration, CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var booking = await context.Bookings
                    .FindAsync(cancellationToken, bookingId)
                    .ConfigureAwait(false);
                booking.Status = status;
                booking.ProposedTime = proposedTime;
                booking.Duration = duration;
            }
        }

        public async Task AddTherapistBooking(
            Guid bookingId, Guid therapistId, DateTime proposedTime,
            CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var booking = await context.Bookings
                    .FindAsync(cancellationToken, bookingId)
                    .ConfigureAwait(false);
                var therapist =
                    new DbTherapistBooking
                    {
                        TherapistBookingId = Guid.NewGuid(),
                        BookingId = bookingId,
                        TherapistId = therapistId,
                        ProposedTime = proposedTime,
                        Status = BookingStatus.BidByTherapist,
                        Booking = booking
                    };
                context.TherapistBookings.Add(therapist);
                await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task UpdateTherapistBooking(
            Guid bookingId, Guid therapistId, DateTime proposedTime, BookingStatus status,
            CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var therapist = await context.TherapistBookings
                    .FirstOrDefaultAsync(tb =>
                        tb.BookingId == bookingId &&
                        tb.TherapistId == therapistId,
                        cancellationToken)
                    .ConfigureAwait(false);
                if (therapist != null)
                {
                    therapist.ProposedTime = proposedTime;
                    therapist.Status = status;

                    await context
                        .SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }
    }
}

