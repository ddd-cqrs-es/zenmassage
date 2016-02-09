using System;
using System.ComponentModel.DataAnnotations;

namespace Zen.Infrastructure.ReadRepository.DataAccess
{
    public class DbBooking
    {
        [Key]
        public Guid BookingId { get; set; }
    }
}