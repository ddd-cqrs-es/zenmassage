using System;
using System.Collections.Generic;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IBooking
    {
        Guid BookingId { get; }

        Guid ClientId { get; }

        ICollection<ITherapistBooking> TherapistBookings { get; }

        BookingStatus Status { get; }

        DateTime ProposedTime { get; }

        TimeSpan Duration { get; }

        void Tender();

        void Bid(Guid therapistId, DateTime? proposedTime);

        void Cancel(string reason);
    }
}
