using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zen.Massage.Site.Controllers.V1
{
    /// <summary>
    /// TODO: Booking is an aggregate with entities that define the booking state per therapist
    /// we will expand the support here to implement lightweight DDD with write and read repos
    /// </summary>
    public interface IBookingRepository
    {
        Task<BookingItem> GetRootBookingAsync(Guid bookingId);

        Task<ICollection<BookingItem>> GetByBookingAsync(Guid bookingId);

        Task<ICollection<BookingItem>> GetByUserAsync(Guid userId);

        Task<ICollection<BookingItem>> GetByBookingAndUserAsync(Guid bookingId, Guid userId);

        Task<ICollection<BookingItem>> GetByBookingAndClientAsync(Guid bookingId, Guid clientId);

        Task<BookingItem> GetByBookingAndTherapistAsync(Guid bookingId, Guid therapistId);

        Task AddAsync(BookingItem booking);

        Task UpdateAsync(BookingItem booking);
    }
}