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

        public ICollection<ITherapistBooking> TherapistBookings => State.AssociatedTherapists;

        public BookingStatus Status => State.Status;

        public DateTime ProposedTime => State.ProposedTime;

        public TimeSpan Duration => State.Duration;

        /// <summary>
        /// Create the booking object
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="proposedTime"></param>
        /// <param name="duration"></param>
        /// <remarks>
        /// This method should never be exposed to the IBooking interface and
        /// is only called by the BookingFactory.
        /// </remarks>
        public void Create(Guid clientId, DateTime proposedTime, TimeSpan duration)
        {
            if (BookingId != Guid.Empty)
            {
                throw new InvalidOperationException("Booking has already been created.");
            }

            ApplyChange(new BookingCreatedEvent(Guid.NewGuid(), clientId, proposedTime, duration));
        }

        public void Tender()
        {
            // Sanity checks
            if (Status == BookingStatus.Tender)
            {
                return;
            }
            if (Status != BookingStatus.Provisional)
            {
                throw new InvalidOperationException("Tender state can only be entered from provisional state.");
            }

            ApplyChange(new BookingTenderEvent(BookingId));
        }

        public void Bid(Guid therapistId, DateTime? proposedTime)
        {
            // TODO: Sanity checks

            ApplyChange(new BookingBidEvent(BookingId, therapistId, proposedTime ?? ProposedTime));
        }

        public void Cancel(string reason)
        {
            // TODO: Sanity checks

            ApplyChange(new BookingCancelledEvent(BookingId, reason));
        }
    }
}
