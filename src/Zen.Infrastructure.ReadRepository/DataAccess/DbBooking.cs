using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zen.Massage.Domain.BookingBoundedContext;

namespace Zen.Infrastructure.ReadRepository.DataAccess
{
    public class DbBooking
    {
        [Key]
        public Guid BookingId { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public BookingStatus Status { get; set; }

        [Required]
        public DateTime ProposedTime { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public virtual ICollection<DbTherapistBooking> TherapistBookings { get; set; }
    }
}