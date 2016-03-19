using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AggregateSource;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public class BookingState : EntityState
    {
        private readonly IList<TherapistBookingEntity> _therapists =
            new List<TherapistBookingEntity>();

        public BookingState()
        {
            Register<BookingCreatedEvent>(OnBookingCreated);
            Register<BookingCancelledEvent>(OnBookingCancelled);
            Register<TherapistBookingAcceptedEvent>(OnBookingAccepted);
            Register<TherapistBookingConfirmedEvent>(OnBookingConfirmed);
        }

        public IBooking Booking { get; set; }

        public Action<object> Applier { get; set; }

        public BookingId BookingId { get; private set; }

        public TenantId TenantId { get; private set; }

        public CustomerId CustomerId { get; private set; }

        public TherapyId TherapyId { get; private set; }

        public ICollection<ITherapistBooking> AssociatedTherapists =>
            new ReadOnlyCollection<ITherapistBooking>(
                _therapists.Cast<ITherapistBooking>().ToList());

        public BookingStatus Status { get; private set; }

        public DateTimeOffset ProposedTime { get; private set; }

        public TimeSpan Duration { get; private set; }

        private void OnBookingCreated(BookingCreatedEvent eventObject)
        {
            BookingId = eventObject.BookingId;
            TenantId = eventObject.TenantId;
            CustomerId = eventObject.CustomerId;
            TherapyId = eventObject.TherapyId;
            Status = BookingStatus.Provisional;
            ProposedTime = eventObject.ProposedTime;
            Duration = eventObject.Duration;
        }

        private void OnBookingBid(BookingBidEvent eventObject)
        {
            // Create therapist object and delegate the event through
            var therapistEntity = new TherapistBookingEntity(Booking, Applier);
            therapistEntity.Route(eventObject);
            _therapists.Add(therapistEntity);
        }

        private void OnBookingCancelled(BookingCancelledEvent eventObject)
        {
            if (eventObject.TherapistId != TherapistId.Empty)
            {
                // Booking cancelled by therapist
                // Route event through to the associated therapist booking
                var therapist = _therapists.FirstOrDefault(t => t.TherapistId == eventObject.TherapistId);
                therapist?.Route(eventObject);

                // If a booking is cancelled by the therapist we probably need to
                //  return to the tender state...
                Status = BookingStatus.Tender;
            }
            else
            {
                // Booking cancelled by client
                Status = BookingStatus.CancelledByClient;
            }
        }

        private void OnBookingAccepted(TherapistBookingAcceptedEvent eventObject)
        {
            var therapist = _therapists.FirstOrDefault(t => t.TherapistId == eventObject.TherapistId);
            if (therapist != null && therapist.Status == BookingStatus.BidByTherapist)
            {
                therapist.Route(eventObject);
                Status = BookingStatus.AcceptByClient;
            }
        }

        private void OnBookingConfirmed(TherapistBookingConfirmedEvent eventObject)
        {
            var therapist = _therapists.FirstOrDefault(t => t.TherapistId == eventObject.TherapistId);
            if (therapist != null && therapist.Status == BookingStatus.AcceptByClient)
            {
                therapist.Route(eventObject);
                Status = BookingStatus.Confirmed;
            }
        }
    }
}