using System;

namespace Zen.Massage.Domain.BookingContext
{
    public interface ITherapistBooking
    {
        IBooking Booking { get; }

        Guid TherapistId { get; }

        DateTime ProposedTime { get; }

        BookingStatus Status { get; }

        void Accept();

        void Confirm();

        void Cancel(string reason);
    }
}