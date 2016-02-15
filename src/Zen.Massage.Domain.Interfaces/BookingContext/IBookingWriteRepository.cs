using System;

namespace Zen.Massage.Domain.BookingContext
{
    public interface IBookingWriteRepository
    {
        IBooking Get(BookingId bookingId);

        IBooking GetOptional(BookingId bookingId);

        void Add(IBooking booking);
    }
}