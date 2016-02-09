using System;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IReadTherapistBooking
    {
        Guid TherapistId { get; }

        BookingStatus Status { get; }

        DateTime ProposedTime { get; }
    }
}