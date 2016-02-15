using System;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IReadTherapistBooking
    {
        TherapistId TherapistId { get; }

        BookingStatus Status { get; }

        DateTime ProposedTime { get; }
    }
}