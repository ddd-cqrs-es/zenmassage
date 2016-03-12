using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AggregateSource;
using Zen.Massage.Domain;
using Zen.Massage.Domain.BookingBoundedContext;
using Zen.Massage.Domain.GeneralBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

namespace Zen.Massage.Application
{
    public class BookingCommandService : IBookingCommandService
    {
        private readonly IBookingFactory _bookingFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public BookingCommandService(
            IBookingFactory bookingFactory,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _bookingFactory = bookingFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public BookingId Create(TenantId tenantId, CustomerId customerId, DateTime proposedTime, TimeSpan duration)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Create booking, add to repo and commit
                var booking = _bookingFactory.Create(tenantId, customerId, proposedTime, duration);
                uow.GetRepository<IBookingWriteRepository>().Add(booking);
                uow.Commit();
                return booking.BookingId;
            }
        }

        public void Tender(TenantId tenantId, BookingId bookingId, CustomerId customerId)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, open for tender and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);

                // Sanity check: Owner of booking must match specified customer
                if (booking.CustomerId != customerId)
                {
                    throw new ArgumentException(
                        "Booking is owned by different customer.",
                        nameof(customerId));

                }

                // Place booking into tendered state and commit
                booking.Tender();
                uow.Commit();
            }
        }

        public void PlaceBid(TenantId tenantId, BookingId bookingId, TherapistId therapistId, DateTime? proposedTime)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, bid and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);
                booking.Bid(therapistId, proposedTime);
                uow.Commit();
            }
        }

        public void AcceptBid(TenantId tenantId, BookingId bookingId, CustomerId customerId, TherapistId therapistId)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, open for tender and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);

                // Sanity check: Owner of booking must match specified customer
                if (booking.CustomerId != customerId)
                {
                    throw new ArgumentException(
                        "Booking is owned by different customer.",
                        nameof(customerId));

                }

                // Find the associated therapist for this booking, accept bid then commit
                var therapist = booking.TherapistBookings.First(t => t.TherapistId == therapistId);
                therapist.Accept();
                uow.Commit();
            }
        }

        public void ConfirmBid(TenantId tenantId, BookingId bookingId, TherapistId therapistId)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, open for tender and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);
                var therapist = booking.TherapistBookings.First(t => t.TherapistId == therapistId);
                therapist.Confirm();
                uow.Commit();
            }
        }

        public void Cancel(TenantId tenantId, BookingId bookingId, CustomerId customerId, string reason)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, open for tender and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);

                // Sanity check: Owner of booking must match specified customer
                if (booking.CustomerId != customerId)
                {
                    throw new ArgumentException(
                        "Booking is owned by different customer.",
                        nameof(customerId));

                }

                booking.Cancel(reason);
                uow.Commit();
            }
        }

        public void Cancel(TenantId tenantId, BookingId bookingId, TherapistId therapistId, string reason)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, open for tender and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);
                var therapist = booking.TherapistBookings.First(t => t.TherapistId == therapistId);
                therapist.Cancel(reason);
                uow.Commit();
            }
        }

    }
}
