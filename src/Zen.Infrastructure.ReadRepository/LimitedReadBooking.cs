using System;
using System.Collections.Generic;
using System.Linq;
using Zen.Massage.Domain.BookingBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Infrastructure.ReadRepository
{
    public class LimitedReadBooking : IReadBooking
    {
        private readonly IReadBooking _sourceBooking;
        private readonly TherapistId _therapistId;

        public LimitedReadBooking(IReadBooking sourceBooking, TherapistId therapistId)
        {
            _sourceBooking = sourceBooking;
            _therapistId = therapistId;
        }

        public BookingId BookingId => _sourceBooking.BookingId;

        public CustomerId CustomerId => _sourceBooking.CustomerId;

        public BookingStatus Status => _sourceBooking.Status;

        public DateTime ProposedTime => _sourceBooking.ProposedTime;

        public TimeSpan Duration => _sourceBooking.Duration;

        public ICollection<IReadTherapistBooking> TherapistBookings
        {
            get
            {
                return _sourceBooking.TherapistBookings
                    .Where(tb => tb.TherapistId == _therapistId)
                    .ToList()
                    .AsReadOnly();
            }
        }

        public IReadBooking LimitToTherapist(TherapistId therapistId)
        {
            if (!TherapistBookings.Any(tb => tb.TherapistId == therapistId))
            {
                return null;
            }

            return this;
        }
    }
}