using System;

namespace Zen.Massage.Application
{
    public interface IBookingCommandService
    {
        void Create(Guid clientId, DateTime proposedTime, TimeSpan duration);

        void Tender(Guid bookingId);

        void PlaceBid(Guid bookingId, Guid therapistId, DateTime? proposedTime);

        void AcceptBid(Guid bookingId, Guid therapistId);

        void ConfirmBid(Guid bookingId, Guid therapistId);

        void Cancel(Guid bookingId, string reason);

        void Cancel(Guid bookingId, Guid therapistId, string reason);
    }
}