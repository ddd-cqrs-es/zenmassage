using System;
using System.Collections.Generic;
using AggregateSource;
using Zen.Massage.Domain.GeneralBoundedContext;
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

        public TenantId TenantId => State.TenantId;

        public CustomerId CustomerId => State.CustomerId;

        public ICollection<ITherapistBooking> TherapistBookings => State.AssociatedTherapists;

        public BookingStatus Status => State.Status;

        public TherapyId TherapyId => State.TherapyId;

        public DateTimeOffset ProposedTime => State.ProposedTime;

        public TimeSpan Duration => State.Duration;

        /// <summary>
        /// Create the booking object
        /// </summary>
        /// <param name="tenantId">Tenant identifier to associate booking with</param>
        /// <param name="customerId">Customer identifier of customer creating booking</param>
        /// <param name="therapyId">Treatment therapy identifier</param>
        /// <param name="proposedTime">Proposed date/time for the treatment</param>
        /// <param name="duration">Duration of the treatment</param>
        /// <remarks>
        /// This method should never be exposed to the IBooking interface and
        /// is only called by the BookingFactory.
        /// </remarks>
        public void Create(TenantId tenantId, CustomerId customerId, TherapyId therapyId, DateTimeOffset proposedTime, TimeSpan duration)
        {
            if (BookingId != BookingId.Empty)
            {
                throw new InvalidOperationException("Booking has already been created.");
            }

            var bookingId = new BookingId(Guid.NewGuid());
            ApplyChange(new BookingCreatedEvent(tenantId, bookingId, customerId, therapyId, proposedTime, duration));
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

            ApplyChange(new BookingTenderEvent(TenantId, BookingId));
        }

        public void Bid(TherapistId therapistId, DateTimeOffset? proposedTime)
        {
            // TODO: Sanity checks

            ApplyChange(new BookingBidEvent(TenantId, BookingId, therapistId, proposedTime ?? ProposedTime));
        }

        public void Cancel(string reason)
        {
            // TODO: Sanity checks

            ApplyChange(new BookingCancelledEvent(TenantId, BookingId, reason));
        }
    }
}
