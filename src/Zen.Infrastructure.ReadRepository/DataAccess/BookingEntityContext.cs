using System.Data.Entity;

namespace Zen.Infrastructure.ReadRepository.DataAccess
{
    public class BookingEntityContext : DbContext
    {
        public BookingEntityContext()
            : base("name=BookingEntityContext")
        {
        }

        public BookingEntityContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<DbBooking> Bookings { get; set; }

        public virtual DbSet<DbTherapistBooking> TherapistBookings { get; set; }
    }
}
