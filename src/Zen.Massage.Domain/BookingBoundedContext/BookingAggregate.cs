using System;
using System.Collections.Generic;
using AggregateSource;
using Zen.Massage.Domain.BookingContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public class BookingAggregate : AggregateRootEntity<BookingState>, IBooking
    {
        public BookingAggregate()
        {
            State.Booking = this;
            State.Applier = ApplyChange;
        }

        public BookingId BookingId => State.BookingId;

        public CustomerId CustomerId => State.CustomerId;

        public ICollection<ITherapistBooking> TherapistBookings => State.AssociatedTherapists;

        public BookingStatus Status => State.Status;

        public DateTime ProposedTime => State.ProposedTime;

        public TimeSpan Duration => State.Duration;

        /// <summary>
        /// Create the booking object
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="proposedTime"></param>
        /// <param name="duration"></param>
        /// <remarks>
        /// This method should never be exposed to the IBooking interface and
        /// is only called by the BookingFactory.
        /// </remarks>
        public void Create(CustomerId customerId, DateTime proposedTime, TimeSpan duration)
        {
            if (BookingId != BookingId.Empty)
            {
                throw new InvalidOperationException("Booking has already been created.");
            }

            var bookingId = new BookingId(Guid.NewGuid());
            ApplyChange(new BookingCreatedEvent(bookingId, customerId, proposedTime, duration));
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

        public void Bid(TherapistId therapistId, DateTime? proposedTime)
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
