using System;
using Zen.Massage.Domain.GeneralBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
    public class TenantEvent
    {
        public TenantEvent(TenantId tenantId)
        {
            TenantId = tenantId;
        }

        public TenantId TenantId { get; private set; }
    }

    [Serializable]
    public class BookingEvent : TenantEvent
    {
        public BookingEvent(TenantId tenantId, BookingId bookingId)
            : base(tenantId)
        {
            BookingId = bookingId;
        }

        public BookingId BookingId { get; private set; }
    }
}