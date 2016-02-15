using System;
using AggregateSource;
using NEventStore;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Infrastructure.WriteRepository
{
    public class BookingWriteRepository : BaseRepository<BookingAggregate, BookingId>, IBookingWriteRepository
    {
        public BookingWriteRepository(UnitOfWork unitOfWork, IStoreEvents eventStore)
            : base(unitOfWork, eventStore)
        {
        }

        public IBooking Get(BookingId bookingId)
        {
            return GetAggregate(bookingId);
        }

        public IBooking GetOptional(BookingId bookingId)
        {
            return GetAggregateOptional(bookingId);
        }

        public void Add(IBooking booking)
        {
            AddAggregate((BookingAggregate)booking);
        }

        protected override BookingId GetKeyFromAggregate(BookingAggregate aggregate)
        {
            return aggregate.BookingId;
        }

        protected override string GetStringKeyRaw(BookingId key)
        {
            return $"{key.Id:N}";
        }
    }
}
