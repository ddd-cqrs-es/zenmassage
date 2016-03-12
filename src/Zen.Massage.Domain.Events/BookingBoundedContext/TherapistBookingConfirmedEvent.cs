using System;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
    public class TherapistBookingConfirmedEvent : TherapistBookingEvent
    {
        public TherapistBookingConfirmedEvent(TenantId tenantId, BookingId bookingId, TherapistId therapistId)
            : base(tenantId, bookingId, therapistId)
        {
        }
    }
}