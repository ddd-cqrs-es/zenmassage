﻿using System;
using AggregateSource;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Domain.BookingBoundedContext
{
    public class TherapistBookingEntityState : EntityState
    {
        public TherapistBookingEntityState()
        {
            Register<BookingBidEvent>(OnBookingBid);
            Register<TherapistBookingAcceptedEvent>(OnBookingAccepted);
            Register<TherapistBookingConfirmedEvent>(OnBookingConfirmed);
            Register<BookingCancelledEvent>(OnBookingCancelled);
        }

        public IBooking Booking { get; set; }

        public TherapistId TherapistId { get; private set; }

        public DateTimeOffset ProposedTime { get; private set; }

        public BookingStatus Status { get; private set; }

        private void OnBookingBid(BookingBidEvent eventObject)
        {
            TherapistId = eventObject.TherapistId;
            ProposedTime = eventObject.ProposedTime;
            Status = BookingStatus.BidByTherapist;
        }

        private void OnBookingAccepted(TherapistBookingAcceptedEvent eventObject)
        {
            Status = BookingStatus.AcceptByClient;
        }

        private void OnBookingConfirmed(TherapistBookingConfirmedEvent eventObject)
        {
            Status = BookingStatus.Confirmed;
        }

        private void OnBookingCancelled(BookingCancelledEvent eventObject)
        {
            Status = BookingStatus.CancelledByTherapist;
        }
    }
}