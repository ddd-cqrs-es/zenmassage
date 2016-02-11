using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AggregateSource;
using Zen.Massage.Domain;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Massage.Application
{
    public interface IBookingService
    {

    }

    public class BookingService : IBookingService
    {
        private readonly IBookingFactory _bookingFactory;
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public BookingService(
            IBookingFactory bookingFactory,
            IBookingReadRepository bookingReadRepository,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _bookingFactory = bookingFactory;
            _bookingReadRepository = bookingReadRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void AddBooking(Guid clientId, DateTime proposedTime, TimeSpan duration)
        {
            using (var uow = _unitOfWorkFactory.CreateSession())
            {
                // Create booking aggregate root object
                var booking = _bookingFactory.Create(clientId, proposedTime, duration);

                // Add booking aggregate to repository
                uow.GetRepository<IBookingWriteRepository>().Add(booking);

                // Commit changes
                uow.Commit();
            }
        }
    }
}
