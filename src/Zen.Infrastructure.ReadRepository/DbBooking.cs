using System;
using System.ComponentModel.DataAnnotations;

namespace Zen.Infrastructure.ReadRepository
{
    public class DbBooking
    {
        [Key]
        public Guid BookingId { get; set; }
    }
}