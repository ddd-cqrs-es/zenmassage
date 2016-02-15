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
        public async Task<IReadBooking> GetBooking(BookingId bookingId, bool includeTherapists, CancellationToken cancellationToken)
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
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId.Id, cancellationToken)
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

        public async Task<IEnumerable<IReadBooking>> GetFutureBookingsForClient(ClientId clientId, DateTime cutoffDate, CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var query = await context.Bookings
                    .Include("TherapistBookings")
                    .Where(b =>
                        b.ProposedTime > cutoffDate &&
                        b.ClientId == clientId.Id &&
                        b.Status != BookingStatus.Provisional &&
                        b.Status != BookingStatus.CancelledByClient)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
                return query
                    .Select(b => new ReadBooking(b));
            }
        }

        public async Task<IEnumerable<IReadBooking>> GetFutureBookingsForTherapist(TherapistId therapistId, DateTime cutoffDate, CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var query = await context.TherapistBookings
                    .Include("Booking")
                    .Where(tb =>
                        tb.Booking.ProposedTime > cutoffDate &&
                        tb.TherapistId == therapistId.Id &&
                        tb.Booking.Status != BookingStatus.Provisional &&
                        tb.Booking.Status != BookingStatus.CancelledByClient)
                    .Select(tb => tb.Booking)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
                return query
                    .Select(b => new ReadBooking(b));
            }
        }

        public async Task AddBooking(BookingId bookingId, ClientId clientId, DateTime proposedTime, TimeSpan duration, CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var booking =
                    new DbBooking
                    {
                        BookingId = bookingId.Id,
                        Status = BookingStatus.Provisional,
                        ClientId = clientId.Id,
                        ProposedTime = proposedTime,
                        Duration = duration
                    };
                context.Bookings.Add(booking);
                await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task UpdateBooking(BookingId bookingId, BookingStatus? status, DateTime? proposedTime, TimeSpan? duration, CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var booking = await context.Bookings
                    .FindAsync(cancellationToken, bookingId.Id)
                    .ConfigureAwait(false);
                if (status.HasValue)
                {
                    booking.Status = status.Value;
                }
                if (proposedTime.HasValue)
                {
                    booking.ProposedTime = proposedTime.Value;
                }
                if (duration.HasValue)
                {
                    booking.Duration = duration.Value;
                }
            }
        }

        public async Task AddTherapistBooking(
            BookingId bookingId, TherapistId therapistId, BookingStatus status, DateTime proposedTime,
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
                        BookingId = bookingId.Id,
                        TherapistId = therapistId.Id,
                        ProposedTime = proposedTime,
                        Status = status,
                        Booking = booking
                    };
                context.TherapistBookings.Add(therapist);
                await context
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task UpdateTherapistBooking(
            BookingId bookingId, TherapistId therapistId, BookingStatus? status, DateTime? proposedTime,
            CancellationToken cancellationToken)
        {
            using (var context = new BookingEntityContext())
            {
                var therapist = await context.TherapistBookings
                    .FirstOrDefaultAsync(tb =>
                        tb.BookingId == bookingId.Id &&
                        tb.TherapistId == therapistId.Id,
                        cancellationToken)
                    .ConfigureAwait(false);
                if (therapist != null)
                {
                    if (status.HasValue)
                    {
                        therapist.Status = status.Value;
                    }
                    if (proposedTime.HasValue)
                    {
                        therapist.ProposedTime = proposedTime.Value;
                    }

                    await context
                        .SaveChangesAsync(cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }
    }
}

