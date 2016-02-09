using System;
using System.ComponentModel.DataAnnotations;

namespace Zen.Infrastructure.ReadRepository.DataAccess
{
    public class DbTherapistBooking
    {
        [Key]
        public Guid BookingId { get; set; }

        [Key]
        public Guid TherapistId { get; set; }
    }
}