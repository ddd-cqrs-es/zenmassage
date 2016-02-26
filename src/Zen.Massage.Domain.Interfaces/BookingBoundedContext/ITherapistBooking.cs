using System;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface ITherapistBooking
    {
        IBooking Booking { get; }

        TherapistId TherapistId { get; }

        DateTime ProposedTime { get; }

        BookingStatus Status { get; }

        void Accept();

        void Confirm();

        void Cancel(string reason);
    }
}