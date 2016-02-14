﻿using System;
using System.Collections.Generic;
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
        [Route("user/{userId}")]
        [SwaggerOperation("GetBookingsByUser")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking retrieved")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to retrieve booking")]
        public async Task<IActionResult> GetUserBookings(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow;
                var clientBookings = await _bookingReadRepository
                    .GetFutureBookingsForClient(userId, cutoffDate, cancellationToken)
                    .ConfigureAwait(true);
                var therapistBookings = await _bookingReadRepository
                    .GetFutureBookingsForTherapist(userId, cutoffDate, cancellationToken)
                    .ConfigureAwait(true);

                var allBookings = clientBookings.Concat(therapistBookings);
                var mappedBookings = allBookings
                    .Select(b => _mapper.Map<BookingItemDto>(b));
                return Ok(
                    new ResultDto<IEnumerable<BookingItemDto>>
                    {
                        StatusCode = 200,
                        StatusDescription = "Bookings for user: " + userId,
                        Result = mappedBookings
                    });
            }
            catch (Exception exception)
            {
                return HttpBadRequest(
                    new ResultDto
                    {
                        StatusCode = 400,
                        StatusDescription = $"Failed to get bookings for user: {exception.Message}"
                    });
            }
        }

        /// <summary>
        /// Gets a single booking using the booking id.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{bookingId}")]
        [SwaggerOperation("GetAllBookings")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking retrieved")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to retrieve booking")]
        public async Task<IActionResult> GetBooking(Guid bookingId, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingReadRepository
                    .GetBooking(bookingId, true, cancellationToken)
                    .ConfigureAwait(true);
                var mappedBooking = _mapper.Map<BookingItemDto>(booking);
                return Ok(
                    new ResultDto<BookingItemDto>
                    {
                        StatusCode = 200,
                        StatusDescription = "Success",
                        Result = mappedBooking
                    });
            }
            catch (Exception exception)
            {
                return HttpBadRequest(
                    new ResultDto
                    {
                        StatusCode = 400,
                        StatusDescription = "Failed to retrieve booking: " + exception.Message
                    });
            }
        }

        /// <summary>
        /// Creates a new booking in the future associated with a given client
        /// </summary>
        /// <param name="clientId">The client identifier (guid)</param>
        /// <param name="proposedTime">The proposed treatment date and time</param>
        /// <param name="duration">Desired treatment duration</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/{clientId:guid}")]
        [SwaggerOperation("CreateBooking")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking created")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to create booking")]
        public IActionResult CreateBooking(
            Guid clientId, 
            [FromBody]DateTime proposedTime,
            [FromBody]TimeSpan duration)
        {
            try
            {
                var bookingId = _bookingCommandService.Create(clientId, proposedTime, duration);
                return CreatedAtAction(
                    "GetBooking",
                    new Dictionary<string, string>
                    {
                        { "bookingId", bookingId.ToString("D") }
                    },
                    new ResultDto
                    {
                        StatusCode = 200,
                        StatusDescription = "Booking created"
                    });
            }
            catch (Exception exception)
            {
                return HttpBadRequest(
                    new ResultDto
                    {
                        StatusCode = 400,
                        StatusDescription = "Failed to create booking: " + exception.Message
                    });
            }
        }

        /// <summary>
        /// Opens a provisional booking for tender by therapists
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{bookingId}/tender")]
        [SwaggerOperation("TenderBooking")]
        [SwaggerResponse(HttpStatusCode.OK, "Booking tendered")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed to tender booking")]
        public IActionResult TenderBooking(Guid bookingId)
        {
            try
            {
                _bookingCommandService.Tender(bookingId);
                return Ok(
                    new ResultDto
                    {
                        StatusCode = 200,
                        StatusDescription = "Booking tendered"
                    });
            }
            catch (Exception exception)
            {
                return HttpBadRequest(
                    new ResultDto
                    {
                        StatusCode = 400,
                        StatusDescription = "Failed to tender booking: " + exception.Message
                    });
            }
        }
    }
}
