using System;
using Zen.Massage.Domain.BookingBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Application
{
    public interface IBookingCommandService
    {
        BookingId Create(CustomerId customerId, DateTime proposedTime, TimeSpan duration);

        void Tender(BookingId bookingId, CustomerId customerId);

        void PlaceBid(BookingId bookingId, TherapistId therapistId, DateTime? proposedTime);

        void AcceptBid(BookingId bookingId, CustomerId customerId, TherapistId therapistId);

        void ConfirmBid(BookingId bookingId, TherapistId therapistId);

        void Cancel(BookingId bookingId, CustomerId customerId, string reason);

        void Cancel(BookingId bookingId, TherapistId therapistId, string reason);
    }
}