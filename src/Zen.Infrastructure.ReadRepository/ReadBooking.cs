using System;
using System.Collections.Generic;
using System.Linq;
using Zen.Infrastructure.ReadRepository.DataAccess;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Infrastructure.ReadRepository
{
    public class ReadBooking : IReadBooking
    {
        private readonly DbBooking _innerBooking;

        public ReadBooking(DbBooking booking)
        {
            _innerBooking = booking;

            TherapistBookings = _innerBooking
                .TherapistBookings
                .Select(tb => (IReadTherapistBooking)new ReadTherapistBooking(tb))
                .ToList()
                .AsReadOnly();
        }

        public Guid BookingId => _innerBooking.BookingId;

        public Guid ClientId => _innerBooking.ClientId;

        public BookingStatus Status => _innerBooking.Status;

        public DateTime ProposedTime => _innerBooking.ProposedTime;

        public TimeSpan Duration => _innerBooking.Duration;

        public ICollection<IReadTherapistBooking> TherapistBookings { get; private set; }
    }
}