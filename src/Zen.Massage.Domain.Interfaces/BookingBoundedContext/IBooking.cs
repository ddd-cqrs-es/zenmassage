using System;
using System.Collections.Generic;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface IBooking
    {
        BookingId BookingId { get; }

        TenantId TenantId { get; }

        CustomerId CustomerId { get; }

        TherapyId TherapyId { get; }

        ICollection<ITherapistBooking> TherapistBookings { get; }

        BookingStatus Status { get; }

        DateTimeOffset ProposedTime { get; }

        TimeSpan Duration { get; }

        void Tender();

        void Bid(TherapistId therapistId, DateTimeOffset? proposedTime);

        void Cancel(string reason);
    }
}
