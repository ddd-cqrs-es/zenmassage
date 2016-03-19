using System;
using AggregateSource;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public class TherapistBookingEntity : Entity<TherapistBookingEntityState>, ITherapistBooking
    {
        public TherapistBookingEntity(IBooking owner, Action<object> applier)
            : base(applier)
        {
            State.Booking = owner;
        }

        public IBooking Booking => State.Booking;

        public TherapistId TherapistId => State.TherapistId;

        public DateTimeOffset ProposedTime => State.ProposedTime;

        public BookingStatus Status => State.Status;

        public void Accept()
        {
            // TODO: Sanity checks

            Apply(new TherapistBookingAcceptedEvent(Booking.TenantId, Booking.BookingId, TherapistId));
        }

        public void Confirm()
        {
            // TODO: Sanity checks

            Apply(new TherapistBookingConfirmedEvent(Booking.TenantId, Booking.BookingId, TherapistId));
        }

        public void Cancel(string reason)
        {
            // TODO: Sanity checks

            Apply(new BookingCancelledEvent(Booking.TenantId, Booking.BookingId, TherapistId, reason));
        }
    }
}
