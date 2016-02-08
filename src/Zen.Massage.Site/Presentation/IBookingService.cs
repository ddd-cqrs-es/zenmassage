using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zen.Massage.Site.Controllers.V1
{
    public interface IBookingService
    {
        Task<ICollection<BookingItem>> GetByBookingId(Guid bookingId);

        Task<ICollection<BookingItem>> GetByUserId(Guid userId);
    }
}