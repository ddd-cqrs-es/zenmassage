using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace Zen.Massage.Site.Controllers.V1
{
    [Route("api/bookings")]
    public class BookingApiController : Controller
    {
    }

    public class BookingItemDto
    {
        public Guid BookingId { get; set; }

        public Guid TherapistId { get; set; }

        public Guid ClientId { get; set; }

    }

    public interface IBookingService
    {
        Task<ICollection<BookingItem>> GetByBookingId(Guid bookingId);

        Task<ICollection<BookingItem>> GetByUserId(Guid userId);
    }

    public class BookingItem
    {
        public Guid BookingId { get; set; }

        public Guid ClientId { get; set; }

        public Guid? TherapistId { get; set; }

        public DateTime CreatedDate { get; set; }

        public BookingStatus Status { get; set; }

        public string CustomerName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? StartTime { get; set; }

        public TimeSpan Duration { get; set; }
    }

    public enum BookingStatus
    {
        Provisional = 0,
        Tender = 1,
        BidByTherapist = 2,
        AcceptByClient = 3,
        Confirmed = 4,
        CancelledByTherapist = 5,
        CancelledByClient = 6,
        Completed = 7
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }

    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public Task<ICollection<BookingItem>> GetByBookingId(Guid bookingId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<BookingItem>> GetByUserId(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task AddBooking(BookingItem booking)
        {
            return Task.FromResult(true);
        }

        public async Task BidAsync(Guid bookingId, Guid therapistId, DateTime? startTime)
        {
            var bookings = await _bookingRepository.GetByBookingAsync(bookingId).ConfigureAwait(false);
            if (bookings.Any(b =>
                b.Status == BookingStatus.Confirmed ||
                b.Status == BookingStatus.CancelledByClient ||
                b.Status == BookingStatus.Completed))
            {
                // Not in a state where we can bid on the treatment
                return;
            }

            // Look for existing booking record for this therapist
            var existing = bookings.FirstOrDefault(b => b.BookingId == bookingId && b.TherapistId == therapistId);
            if (existing != null)
            {
                // If existing booking (there will only be one) is cancelled then reset state
                if (existing.Status == BookingStatus.CancelledByTherapist)
                {
                    existing.Status = BookingStatus.BidByTherapist;
                    await _bookingRepository.UpdateAsync(existing).ConfigureAwait(false);
                }

                return;
            }

            // Initial booking
            var initialBooking = bookings.FirstOrDefault(b => b.BookingId == bookingId && therapistId == null);

            // Clone the booking and add entry for this therapist
            var therapistBooking = CloneBookingFor(initialBooking, therapistId, BookingStatus.BidByTherapist, startTime);
            await _bookingRepository.AddAsync(therapistBooking).ConfigureAwait(false);
        }

        private static readonly IReadOnlyDictionary<BookingStatus, bool> CanConfirmStates =
            new Dictionary<BookingStatus, bool>
            {
                {BookingStatus.Provisional, false},
                {BookingStatus.Tender, false},
                {BookingStatus.BidByTherapist, false},
                {BookingStatus.AcceptByClient, false},
                {BookingStatus.Confirmed, false},
                {BookingStatus.CancelledByClient, false},
                {BookingStatus.CancelledByTherapist, false},
                {BookingStatus.Completed, false},
            };

        public async Task<int> ConfirmAsync(Guid bookingId, Guid clientId, Guid therapistId)
        {
            var bookings = await _bookingRepository.GetByBookingAsync(bookingId).ConfigureAwait(false);
            if (bookings.Any(t => t.Status == BookingStatus.Confirmed))
            {
                return 1; // Already confirmed
            }
            if (!bookings.Any(t => t.Status == BookingStatus.BidByTherapist))
            {
                return 2; // Never bid for by therapist
            }
            return 0;
        }

        public Task CancelAsync(Guid bookingId, Guid userId)
        {
            throw new NotImplementedException();
        }

        private BookingItem CloneBookingFor(BookingItem initialBooking, Guid therapistId, BookingStatus status, DateTime? startTime = null)
        {
            return
                new BookingItem
                {
                    BookingId = initialBooking.BookingId,
                    ClientId = initialBooking.ClientId,
                    TherapistId = therapistId,
                    CreatedDate = initialBooking.CreatedDate,
                    Status = status,
                    CustomerName = initialBooking.CustomerName,
                    Gender = initialBooking.Gender,
                    StartTime = startTime ?? initialBooking.StartTime,
                    Duration = initialBooking.Duration
                };
        }
    }

    public interface IBookingRepository
    {
        Task<ICollection<BookingItem>> GetByBookingAsync(Guid bookingId);

        Task<ICollection<BookingItem>> GetByUserAsync(Guid userId);

        Task<ICollection<BookingItem>> GetByBookingAndUserAsync(Guid bookingId, Guid userId);

        Task AddAsync(BookingItem booking);

        Task UpdateAsync(BookingItem booking);
    }

    public class BookingRepository : IBookingRepository
    {
        public Task<ICollection<BookingItem>> GetByBookingAsync(Guid bookingId)
        {
            return Task.FromResult<ICollection<BookingItem>>(new BookingItem[0]);
        }

        public Task<ICollection<BookingItem>> GetByUserAsync(Guid userId)
        {
            return Task.FromResult<ICollection<BookingItem>>(new BookingItem[0]);
        }

        public Task<ICollection<BookingItem>> GetByBookingAndUserAsync(Guid bookingId, Guid userId)
        {
            return Task.FromResult<ICollection<BookingItem>>(new BookingItem[0]);
        }

        public Task AddAsync(BookingItem booking)
        {
            return Task.FromResult(true);
        }

        public Task UpdateAsync(BookingItem booking)
        {
            return Task.FromResult(true);
        }
    }
}
