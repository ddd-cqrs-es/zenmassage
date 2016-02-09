using System;
using System.Collections.Generic;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IReadBooking
    {
        Guid BookingId { get; }

        Guid ClientId { get; }

        BookingStatus Status { get; }

        DateTime ProposedTime { get; }

        TimeSpan Duration { get; }

        ICollection<IReadTherapistBooking> TherapistBookings { get; }
    }
}