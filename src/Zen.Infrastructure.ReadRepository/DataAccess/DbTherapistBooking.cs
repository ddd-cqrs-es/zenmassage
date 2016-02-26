using System;
using System.ComponentModel.DataAnnotations;
using Zen.Massage.Domain.BookingBoundedContext;

namespace Zen.Infrastructure.ReadRepository.DataAccess
{
    public class DbTherapistBooking
    {
        [Key]
        public Guid TherapistBookingId { get; set; }

        public Guid BookingId { get; set; }

        [Required]
        public Guid TherapistId { get; set; }

        [Required]
        public BookingStatus Status { get; set; }

        [Required]
        public DateTime ProposedTime { get; set; }

        public virtual DbBooking Booking { get; set; }
    }
}