using System;
using Zen.Massage.Domain.BookingBoundedContext;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Application
{
    public interface IBookingCommandService
    {
        BookingId Create(TenantId tenantId, CustomerId customerId, TherapyId therapyId, DateTimeOffset proposedTime, TimeSpan duration);

        void Tender(TenantId tenantId, BookingId bookingId, CustomerId customerId);

        void PlaceBid(TenantId tenantId, BookingId bookingId, TherapistId therapistId, DateTime? proposedTime);

        void AcceptBid(TenantId tenantId, BookingId bookingId, CustomerId customerId, TherapistId therapistId);

        void ConfirmBid(TenantId tenantId, BookingId bookingId, TherapistId therapistId);

        void Cancel(TenantId tenantId, BookingId bookingId, CustomerId customerId, string reason);

        void Cancel(TenantId tenantId, BookingId bookingId, TherapistId therapistId, string reason);
    }
}