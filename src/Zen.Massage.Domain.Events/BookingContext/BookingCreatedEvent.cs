using System;
using Zen.Massage.Domain.BookingBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingContext
{
    [Serializable]
    public class BookingCreatedEvent : BookingEvent
    {
        public BookingCreatedEvent(
            BookingId bookingId,
            CustomerId customerId,
            DateTime proposedTime,
            TimeSpan duration)
            : base(bookingId)
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