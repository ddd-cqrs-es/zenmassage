using System;
using Zen.Massage.Domain.GeneralBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
    public class BookingTenderEvent : BookingEvent
    {
        public BookingTenderEvent(TenantId tenantId, BookingId bookingId)
            : base(tenantId, bookingId)
        {
        }
    }
}