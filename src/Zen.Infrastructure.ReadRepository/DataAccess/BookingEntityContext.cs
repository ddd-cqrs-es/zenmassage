using Microsoft.Data.Entity;

namespace Zen.Infrastructure.ReadRepository.DataAccess
{
    public class BookingEntityContext : DbContext
    {
        private readonly string _connectionString;

        public BookingEntityContext()
        {
        }

        public BookingEntityContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<DbBooking> Bookings { get; set; }

        public virtual DbSet<DbTherapistBooking> TherapistBookings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                builder.UseSqlServer(_connectionString);
            }
        }
    }
}
