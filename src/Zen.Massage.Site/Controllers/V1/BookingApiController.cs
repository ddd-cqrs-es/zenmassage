using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using Zen.Massage.Application;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Massage.Site.Controllers.V1
{
    /// <summary>
    /// Booking endpoint
    /// </summary>
    /// <remarks>
    /// This endpoint exposes information regarding bookings.
    /// Ultimately access to the information here will be limited by user id
    /// as supplied by an OAuth2 claim from our identity server but for now
    /// it is wide open!
    /// </remarks>
    [Route("api/v1/bookings")]
    public class BookingApiControllerV1 : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingCommandService _bookingCommandService;

        public BookingApiControllerV1(
            MapperConfiguration mapperConfiguration,
            IBookingReadRepository bookingReadRepository,
            IBookingCommandService bookingCommandService)
        {
            _mapper = mapperConfiguration.CreateMapper();
            _bookingReadRepository = bookingReadRepository;
            _bookingCommandService = bookingCommandService;
        }

        /// <summary>
        /// Gets bookings associated with a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/{userId:guid}")]
        [SwaggerOperation("GetBookingsByUser")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking retrieved")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to retrieve booking")]
        public async Task<IActionResult> GetUserBookings(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow;
                var clientBookings = await _bookingReadRepository
                    .GetFutureBookingsForClient(new ClientId(userId), cutoffDate, cancellationToken)
                    .ConfigureAwait(true);
                var therapistBookings = await _bookingReadRepository
                    .GetFutureBookingsForTherapist(new TherapistId(userId), cutoffDate, cancellationToken)
                    .ConfigureAwait(true);

                var allBookings = clientBookings.Concat(therapistBookings);
                var mappedBookings = allBookings
                    .Select(b => _mapper.Map<BookingItemDto>(b));
                return Ok(mappedBookings);
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to get bookings for user: {exception.Message}");
            }
        }

        /// <summary>
        /// Gets a single booking using the booking id.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{bookingId:guid}")]
        [SwaggerOperation("GetAllBookings")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking retrieved")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to retrieve booking")]
        public async Task<IActionResult> GetBooking(Guid bookingId, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingReadRepository
                    .GetBooking(new BookingId(bookingId), true, cancellationToken)
                    .ConfigureAwait(true);
                var mappedBooking = _mapper.Map<BookingItemDto>(booking);
                return Ok(mappedBooking);
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to retrieve booking: {exception.Message}");
            }
        }

        /// <summary>
        /// Creates a new booking in the future associated with a given client
        /// </summary>
        /// <param name="clientId">The client identifier (guid)</param>
        /// <param name="booking">The booking information</param>
        /// <returns>
        /// HTTP Created with the booking id.
        /// </returns>
        /// <remarks>
        /// The url that represents the booking object via this API is returned
        /// in the location header of the response.
        /// </remarks>
        [HttpPost]
        [Route("user/{clientId:guid}")]
        [SwaggerOperation("CreateBooking")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking created")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to create booking")]
        public IActionResult CreateBooking(
            Guid clientId, 
            [FromBody]CreateBookingDto booking)
        {
            try
            {
                var bookingId = _bookingCommandService.Create(new ClientId(clientId), booking.ProposedTime, booking.Duration);
                return Created(new Uri($"http://localhost:1282/api/v1/bookings/{bookingId.Id:D}/"), bookingId.Id);
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to create booking: {exception.Message}");
            }
        }

        /// <summary>
        /// Opens a provisional booking for tender by therapists
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{bookingId:guid}/tender")]
        [SwaggerOperation("TenderBooking")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking tendered")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to tender booking")]
        public IActionResult TenderBooking(Guid bookingId)
        {
            try
            {
                _bookingCommandService.Tender(new BookingId(bookingId));
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to tender booking: {exception.Message}");
            }
        }
    }
}
