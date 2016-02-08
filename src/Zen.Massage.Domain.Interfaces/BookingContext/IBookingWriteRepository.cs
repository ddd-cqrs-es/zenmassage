using System;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IBookingWriteRepository
    {
        IBooking Get(Guid bookingId);

        IBooking GetOptional(Guid bookingId);

        void Add(IBooking booking);
    }
}