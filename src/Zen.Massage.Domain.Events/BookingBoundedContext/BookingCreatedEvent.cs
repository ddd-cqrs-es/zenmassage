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
            TherapyId therapyId,
            DateTimeOffset proposedTime,
            TimeSpan duration)
            : base(tenantId, bookingId)
        {
            CustomerId = customerId;
            TherapyId = therapyId;
            ProposedTime = proposedTime;
            Duration = duration;
        }

        public CustomerId CustomerId { get; private set; }

        public TherapyId TherapyId { get; private set; }

        public DateTimeOffset ProposedTime { get; private set; }

        public TimeSpan Duration { get; private set; }
    }
}