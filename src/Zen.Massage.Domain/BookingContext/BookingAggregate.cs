using System;
using System.Collections.Generic;
using AggregateSource;

namespace Zen.Massage.Domain.BookingContext
{
    public class BookingAggregate : AggregateRootEntity<BookingState>, IBooking
    {
        public BookingAggregate()
        {
            State.Booking = this;
            State.Applier = ApplyChange;
        }

        public Guid BookingId => State.BookingId;

        public Guid ClientId => State.ClientId;

        public ICollection<ITherapistBooking> AssociatedTherapists => State.AssociatedTherapists;

        public BookingStatus Status => State.Status;

        public DateTime ProposedTime => State.ProposedTime;

        public TimeSpan Duration => State.Duration;

        public void Tender()
        {
            // TODO: Sanity checks

            ApplyChange(new BookingTenderEvent(BookingId));
        }

        public void Bid(Guid therapistId, DateTime? proposedTime)
        {
            // TODO: Sanity checks

            ApplyChange(new BookingBidEvent(BookingId, therapistId, proposedTime));
        }

        public void Cancel(string reason)
        {
            // TODO: Sanity checks

            ApplyChange(new BookingCancelledEvent(BookingId, reason));
        }
    }
}
