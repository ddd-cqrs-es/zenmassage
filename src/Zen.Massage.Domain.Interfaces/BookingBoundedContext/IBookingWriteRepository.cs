namespace Zen.Massage.Domain.BookingBoundedContext
{
    public interface IBookingWriteRepository
    {
        IBooking Get(BookingId bookingId);

        IBooking GetOptional(BookingId bookingId);

        void Add(IBooking booking);
    }
}