using System;
using AggregateSource;

namespace Zen.Massage.Domain.BookingContext
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

        public DateTime ProposedTime => State.ProposedTime;

        public BookingStatus Status => State.Status;

        public void Accept()
        {
            // TODO: Sanity checks

            Apply(new TherapistBookingAcceptedEvent(Booking.BookingId, TherapistId));
        }

        public void Confirm()
        {
            // TODO: Sanity checks

            Apply(new TherapistBookingConfirmedEvent(Booking.BookingId, TherapistId));
        }

        public void Cancel(string reason)
        {
            // TODO: Sanity checks

            Apply(new BookingCancelledEvent(Booking.BookingId, TherapistId, reason));
        }
    }
}
