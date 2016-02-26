using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using Zen.Massage.Application;
using Zen.Massage.Domain.BookingBoundedContext;
using Zen.Massage.Domain.UserBoundedContext;

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
        [SwaggerResponse(HttpStatusCode.OK, "Bookings retrieved")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to retrieve bookings")]
        public async Task<IActionResult> GetUserBookings(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow;
                var clientBookings = await _bookingReadRepository
                    .GetFutureBookingsForCustomer(new CustomerId(userId), cutoffDate, cancellationToken)
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
        /// Gets bookings associated with a client
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("client/{CustomerId:guid}")]
        [SwaggerOperation("GetBookingsByClient")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookings retrieved")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to retrieve bookings")]
        public async Task<IActionResult> GetClientBookings(Guid clientId, CancellationToken cancellationToken)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow;
                var clientBookings = await _bookingReadRepository
                    .GetFutureBookingsForCustomer(new CustomerId(clientId), cutoffDate, cancellationToken)
                    .ConfigureAwait(true);

                var mappedBookings = clientBookings
                    .Select(b => _mapper.Map<BookingItemDto>(b));
                return Ok(mappedBookings);
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to get bookings for client: {exception.Message}");
            }
        }

        /// <summary>
        /// Gets bookings associated with a therapist
        /// </summary>
        /// <param name="therapistId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("therapist/{therapistId:guid}")]
        [SwaggerOperation("GetBookingsByTherapist")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookings retrieved")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to retrieve bookings")]
        public async Task<IActionResult> GetTherapistBookings(Guid therapistId, CancellationToken cancellationToken)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow;
                var therapistBookings = await _bookingReadRepository
                    .GetFutureBookingsForTherapist(new TherapistId(therapistId), cutoffDate, cancellationToken)
                    .ConfigureAwait(true);

                var mappedBookings = therapistBookings
                    .Select(b => _mapper.Map<BookingItemDto>(b));
                return Ok(mappedBookings);
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to get bookings for therapist: {exception.Message}");
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
        [Route("user/{CustomerId:guid}")]
        [SwaggerOperation("CreateBooking")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking created")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to create booking")]
        public IActionResult CreateBooking(
            Guid clientId, 
            [FromBody]CreateBookingDto booking)
        {
            try
            {
                var bookingId = _bookingCommandService.Create(new CustomerId(clientId), booking.ProposedTime, booking.Duration);
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

        /// <summary>
        /// Bids on a tendered booking
        /// </summary>
        /// <param name="bookingId">Booking identifier</param>
        /// <param name="therapistId">Therapist identifier</param>
        /// <param name="placeBid">Bid placement information</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{bookingId:guid}/bid/{therapistId:guid}")]
        [SwaggerOperation("PlaceBid")]
        [SwaggerResponse(HttpStatusCode.OK, "Bid placed")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to place bid")]
        public IActionResult PlaceBid(
            Guid bookingId,
            Guid therapistId,
            [FromBody] PlaceBookingBidDto placeBid)
        {
            try
            {
                _bookingCommandService.PlaceBid(
                    new BookingId(bookingId),
                    new TherapistId(therapistId),
                    placeBid.ProposedTime);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to place bid on booking: {exception.Message}");
            }
        }

        /// <summary>
        /// Accepts a bid offered by a therapist
        /// </summary>
        /// <param name="bookingId">Booking identifier</param>
        /// <param name="therapistId">Therapist identifier</param>
        /// <param name="placeBid">Bid placement information</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{bookingId:guid}/accept/{therapistId:guid}")]
        [SwaggerOperation("AcceptBid")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking accepted")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to accept bid")]
        public IActionResult AcceptBid(Guid bookingId, Guid therapistId)
        {
            try
            {
                _bookingCommandService.AcceptBid(
                    new BookingId(bookingId),
                    new TherapistId(therapistId));
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to accept booking: {exception.Message}");
            }
        }

        /// <summary>
        /// Confirm an accepted bid on a booking
        /// </summary>
        /// <param name="bookingId">Booking identifier</param>
        /// <param name="therapistId">Therapist identifier</param>
        /// <param name="placeBid">Bid placement information</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{bookingId:guid}/confirm/{therapistId:guid}")]
        [SwaggerOperation("ConfirmedBid")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking confirmed")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to confirm bid")]
        public IActionResult ConfirmBid(Guid bookingId, Guid therapistId)
        {
            try
            {
                _bookingCommandService.ConfirmBid(
                    new BookingId(bookingId),
                    new TherapistId(therapistId));
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to confirm booking: {exception.Message}");
            }
        }

        /// <summary>
        /// Cancels a booking
        /// </summary>
        /// <param name="bookingId">Booking identifier</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{bookingId:guid}/cancel")]
        [SwaggerOperation("CancelBookingByClient")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking cancelled")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to cancel booking")]
        public IActionResult CancelBooking(Guid bookingId)
        {
            try
            {
                _bookingCommandService.Cancel(new BookingId(bookingId), string.Empty);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to cancel booking: {exception.Message}");
            }
        }

        /// <summary>
        /// Cancels a booking (by therapist)
        /// </summary>
        /// <param name="bookingId">Booking identifier</param>
        /// <param name="therapistId">Therapist identifier</param>
        /// <param name="placeBid">Bid placement information</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{bookingId:guid}/cancel/{therapistId:guid}")]
        [SwaggerOperation("CancelBookingByTherapist")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking cancelled")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to cancel booking")]
        public IActionResult CancelBooking(Guid bookingId, Guid therapistId)
        {
            try
            {
                _bookingCommandService.Cancel(
                    new BookingId(bookingId),
                    new TherapistId(therapistId),
                    string.Empty);
                return Ok();
            }
            catch (Exception exception)
            {
                return HttpBadRequest($"Failed to cancel booking: {exception.Message}");
            }
        }
    }
}
