using System;
using Zen.Infrastructure.ReadRepository.DataAccess;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Infrastructure.ReadRepository
{
    public class ReadTherapistBooking : IReadTherapistBooking
    {
        private readonly DbTherapistBooking _innerBooking;

        public ReadTherapistBooking(DbTherapistBooking booking)
        {
            _innerBooking = booking;
        }

        public Guid TherapistId => _innerBooking.TherapistId;

        public BookingStatus Status => _innerBooking.Status;

        public DateTime ProposedTime => _innerBooking.ProposedTime;
    }
}