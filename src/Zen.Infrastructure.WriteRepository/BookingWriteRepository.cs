using System;
using AggregateSource;
using NEventStore;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Infrastructure.WriteRepository
{
    public class BookingWriteRepository : BaseRepository<BookingAggregate, Guid>, IBookingWriteRepository
    {
        public BookingWriteRepository(UnitOfWork unitOfWork, IStoreEvents eventStore)
            : base(unitOfWork, eventStore)
        {
        }

        public IBooking Get(Guid bookingId)
        {
            return GetAggregate(bookingId);
        }

        public IBooking GetOptional(Guid bookingId)
        {
            return GetAggregateOptional(bookingId);
        }

        public void Add(IBooking booking)
        {
            AddAggregate((BookingAggregate)booking);
        }

        protected override Guid GetKeyFromAggregate(BookingAggregate aggregate)
        {
            return aggregate.BookingId;
        }

        protected override string GetStringKeyFromBlob(Guid key)
        {
            return $"Booking:{key:N}";
        }
    }
}
