using System;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Massage.Application
{
    public interface IBookingCommandService
    {
        BookingId Create(ClientId clientId, DateTime proposedTime, TimeSpan duration);

        void Tender(BookingId bookingId);

        void PlaceBid(BookingId bookingId, TherapistId therapistId, DateTime? proposedTime);

        void AcceptBid(BookingId bookingId, TherapistId therapistId);

        void ConfirmBid(BookingId bookingId, TherapistId therapistId);

        void Cancel(BookingId bookingId, string reason);

        void Cancel(BookingId bookingId, TherapistId therapistId, string reason);
    }
}