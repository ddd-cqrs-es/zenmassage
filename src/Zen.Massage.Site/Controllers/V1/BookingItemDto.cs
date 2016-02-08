using System;

namespace Zen.Massage.Site.Controllers.V1
{
    public class BookingItemDto
    {
        public Guid BookingId { get; set; }

        public Guid TherapistId { get; set; }

        public Guid ClientId { get; set; }

    }
}