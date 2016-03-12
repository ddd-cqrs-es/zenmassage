using System;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    [Serializable]
    public class BookingCreatedEvent : BookingEvent
    {
        public BookingCreatedEvent(
            TenantId tenantId,
            BookingId bookingId,
            CustomerId customerId,
            DateTime proposedTime,
            TimeSpan duration)
            : base(tenantId, bookingId)
        {
            CustomerId = customerId;
            ProposedTime = proposedTime;
            Duration = duration;
        }

        public CustomerId CustomerId { get; private set; }

        public DateTime ProposedTime { get; private set; }

        public TimeSpan Duration { get; private set; }
    }
}