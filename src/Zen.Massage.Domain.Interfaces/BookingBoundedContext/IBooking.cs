using System;
using System.Collections.Generic;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface IBooking
    {
        BookingId BookingId { get; }

        CustomerId CustomerId { get; }

        ICollection<ITherapistBooking> TherapistBookings { get; }

        BookingStatus Status { get; }

        DateTime ProposedTime { get; }

        TimeSpan Duration { get; }

        void Tender();

        void Bid(TherapistId therapistId, DateTime? proposedTime);

        void Cancel(string reason);
    }
}
