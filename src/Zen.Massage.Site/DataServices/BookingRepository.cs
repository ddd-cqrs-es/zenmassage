using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zen.Massage.Site.Controllers.V1
{
    public class BookingRepository : IBookingRepository
    {
        private readonly List<BookingItem> _bookings =
            new List<BookingItem>();

        public Task<BookingItem> GetRootBookingAsync(Guid bookingId)
        {
            var booking = _bookings
                .FirstOrDefault(b => b.BookingId == bookingId && b.TherapistId == null);
            return Task.FromResult(booking);
        }

        public Task<ICollection<BookingItem>> GetByBookingAsync(Guid bookingId)
        {
            var bookings = _bookings
                .Where(b => b.BookingId == bookingId)
                .ToList();
            return Task.FromResult<ICollection<BookingItem>>(bookings);
        }

        public Task<ICollection<BookingItem>> GetByUserAsync(Guid userId)
        {
            var bookings = _bookings
                .Where(b => b.ClientId == userId || b.TherapistId == userId)
                .ToList();
            return Task.FromResult<ICollection<BookingItem>>(bookings);
        }

        public Task<ICollection<BookingItem>> GetByBookingAndUserAsync(Guid bookingId, Guid userId)
        {
            var bookings = _bookings
                .Where(b => b.BookingId == bookingId && (b.ClientId == userId || b.TherapistId == userId))
                .ToList();
            return Task.FromResult<ICollection<BookingItem>>(bookings);
        }

        public Task<ICollection<BookingItem>> GetByBookingAndClientAsync(Guid bookingId, Guid clientId)
        {
            var bookings = _bookings
                .Where(b => b.BookingId == bookingId && b.ClientId == clientId)
                .ToList();
            return Task.FromResult<ICollection<BookingItem>>(bookings);
        }

        public Task<BookingItem> GetByBookingAndTherapistAsync(Guid bookingId, Guid therapistId)
        {
            var booking = _bookings
                .FirstOrDefault(b => b.BookingId == bookingId && b.TherapistId == therapistId);
            return Task.FromResult(booking);
        }

        public Task AddAsync(BookingItem booking)
        {
            booking.EntryId = Guid.NewGuid();
            _bookings.Add(booking);
            return Task.FromResult(true);
        }

        public Task UpdateAsync(BookingItem booking)
        {
            return Task.FromResult(true);
        }
    }
}