using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AggregateSource;
using Zen.Massage.Domain;
using Zen.Massage.Domain.BookingContext;

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

        public void Create(Guid clientId, DateTime proposedTime, TimeSpan duration)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Create booking, add to repo and commit
                var booking = _bookingFactory.Create(clientId, proposedTime, duration);
                uow.GetRepository<IBookingWriteRepository>().Add(booking);
                uow.Commit();
            }
        }

        public void Tender(Guid bookingId)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, open for tender and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);
                booking.Tender();
                uow.Commit();
            }
        }

        public void PlaceBid(Guid bookingId, Guid therapistId, DateTime? proposedTime)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, bid and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);
                booking.Bid(therapistId, proposedTime);
                uow.Commit();
            }
        }

        public void AcceptBid(Guid bookingId, Guid therapistId)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, open for tender and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);
                var therapist = booking.AssociatedTherapists.First(t => t.TherapistId == therapistId);
                therapist.Accept();
                uow.Commit();
            }
        }

        public void ConfirmBid(Guid bookingId, Guid therapistId)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, open for tender and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);
                var therapist = booking.AssociatedTherapists.First(t => t.TherapistId == therapistId);
                therapist.Confirm();
                uow.Commit();
            }
        }

        public void Cancel(Guid bookingId, string reason)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, open for tender and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);
                booking.Cancel(reason);
                uow.Commit();
            }
        }

        public void Cancel(Guid bookingId, Guid therapistId, string reason)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Get booking, open for tender and commit
                var booking = uow.GetRepository<IBookingWriteRepository>().Get(bookingId);
                var therapist = booking.AssociatedTherapists.First(t => t.TherapistId == therapistId);
                therapist.Cancel(reason);
                uow.Commit();
            }
        }

    }
}
