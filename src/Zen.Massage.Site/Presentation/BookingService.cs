using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zen.Massage.Site.Controllers.V1
{
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

            // Initial booking
            var initialBooking = bookings.FirstOrDefault(b => b.TherapistId == null);
            if (initialBooking == null)
            {
                return;
            }

            // Look for existing booking record for this therapist
            var existing = bookings.FirstOrDefault(b => b.TherapistId == therapistId);
            if (existing != null)
            {
                // If existing booking (there will only be one) is cancelled then reset state
                if (existing.Status == BookingStatus.CancelledByTherapist)
                {
                    existing.Status = BookingStatus.BidByTherapist;
                }

                // Update the start time if necessary
                existing.StartTime = startTime ?? initialBooking.StartTime;
                await _bookingRepository.UpdateAsync(existing).ConfigureAwait(false);
                return;
            }

            // Clone the booking and add entry for this therapist
            var therapistBooking = 
                new BookingItem
                {
                    BookingId = initialBooking.BookingId,
                    ClientId = initialBooking.ClientId,
                    TherapistId = therapistId,
                    CreatedDate = initialBooking.CreatedDate,
                    Status = BookingStatus.BidByTherapist,
                    CustomerName = initialBooking.CustomerName,
                    Gender = initialBooking.Gender,
                    StartTime = startTime ?? initialBooking.StartTime,
                    Duration = initialBooking.Duration
                };
            await _bookingRepository.AddAsync(therapistBooking).ConfigureAwait(false);
        }

        public async Task AcceptAsync(Guid bookingId, Guid therapistId)
        {
            var booking = await _bookingRepository.GetByBookingAndTherapistAsync(bookingId, therapistId).ConfigureAwait(false);
            if (booking?.Status == BookingStatus.BidByTherapist)
            {
                booking.Status = BookingStatus.AcceptByClient;
                await _bookingRepository.UpdateAsync(booking).ConfigureAwait(false);
            }
        }

        public async Task<int> ConfirmAsync(Guid bookingId, Guid clientId, Guid therapistId)
        {
            var bookings = await _bookingRepository.GetByBookingAsync(bookingId).ConfigureAwait(false);
            var booking = bookings.FirstOrDefault(t => t.Status == BookingStatus.AcceptByClient);
            if (booking == null)
            {
                return 2; // Never accepted by client
            }

            // Update bid to confirm
            booking.Status = BookingStatus.Confirmed;
            await _bookingRepository.UpdateAsync(booking).ConfigureAwait(false);
            return 0;
        }

        public Task CancelAsync(Guid bookingId, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}