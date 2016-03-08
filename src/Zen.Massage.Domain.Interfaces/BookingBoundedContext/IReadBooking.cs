using System;
using System.Collections.Generic;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface IReadBooking
    {
        BookingId BookingId { get; }

        CustomerId CustomerId { get; }

        BookingStatus Status { get; }

        DateTime ProposedTime { get; }

        TimeSpan Duration { get; }

        ICollection<IReadTherapistBooking> TherapistBookings { get; }

        IReadBooking LimitToTherapist(TherapistId therapistId);
    }
}