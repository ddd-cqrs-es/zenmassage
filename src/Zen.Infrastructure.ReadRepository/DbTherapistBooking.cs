using System;
using System.ComponentModel.DataAnnotations;

namespace Zen.Infrastructure.ReadRepository
{
    public class DbTherapistBooking
    {
        [Key]
        public Guid BookingId { get; set; }

        [Key]
        public Guid TherapistId { get; set; }
    }
}