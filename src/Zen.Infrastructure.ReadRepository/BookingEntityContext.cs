using System.Data.Entity;

namespace Zen.Infrastructure.ReadRepository
{
    public class BookingEntityContext : DbContext
    {
        public virtual DbSet<DbBooking> Bookings { get; set; }

        public virtual DbSet<DbTherapistBooking> TherapistBookings { get; set; }
    }
}
