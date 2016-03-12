using System;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
    public class BookingCancelledEvent : BookingEvent
    {
        public BookingCancelledEvent(TenantId tenantId, BookingId bookingId, string reason)
            : this(tenantId, bookingId, TherapistId.Empty, reason)
        {
        }

        public BookingCancelledEvent(TenantId tenantId, BookingId bookingId, TherapistId therapistId, string reason)
            : base(tenantId, bookingId)
        {
            TherapistId = therapistId;
            Reason = reason;
        }

        public TherapistId TherapistId { get; private set; }

        public string Reason { get; private set; }
    }
}
