using System;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
    public class TherapistBookingEvent : BookingEvent
    {
        public TherapistBookingEvent(TenantId tenantId, BookingId bookingId, TherapistId therapistId)
            : base(tenantId, bookingId)
        {
            TherapistId = therapistId;
        }

        public TherapistId TherapistId { get; private set; }
    }
}
