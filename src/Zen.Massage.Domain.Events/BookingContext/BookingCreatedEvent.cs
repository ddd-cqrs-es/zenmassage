using System;

namespace Zen.Massage.Domain.BookingContext
{
    public class BookingCreatedEvent : BookingEvent
    {
        public BookingCreatedEvent(
            Guid bookingId,
            Guid clientId,
            DateTime proposedTime,
            TimeSpan duration)
            : base(bookingId)
        {
            ClientId = clientId;
            ProposedTime = proposedTime;
            Duration = duration;
        }

        public Guid ClientId { get; private set; }

        public DateTime ProposedTime { get; private set; }

        public TimeSpan Duration { get; private set; }
    }
}