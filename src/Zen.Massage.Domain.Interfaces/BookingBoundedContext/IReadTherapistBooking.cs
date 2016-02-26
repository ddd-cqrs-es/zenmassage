using System;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface IReadTherapistBooking
    {
        TherapistId TherapistId { get; }

        BookingStatus Status { get; }

        DateTime ProposedTime { get; }
    }
}