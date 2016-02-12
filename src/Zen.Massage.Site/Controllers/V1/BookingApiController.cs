using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using Zen.Massage.Application;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Massage.Site.Controllers.V1
{
    [Route("api/bookings")]
    public class BookingApiController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBookingReadRepository _bookingReadRepository;
        private readonly IBookingCommandService _bookingCommandService;

        public BookingApiController(
            MapperConfiguration mapperConfiguration,
            IBookingReadRepository bookingReadRepository,
            IBookingCommandService bookingCommandService)
        {
            _mapper = mapperConfiguration.CreateMapper();
            _bookingReadRepository = bookingReadRepository;
            _bookingCommandService = bookingCommandService;
        }

        [HttpGet("user/{userId}")]
        public Task<IActionResult> GetUserBookings(Guid userId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{bookingId}")]
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

        [HttpPost("user/{clientId}")]
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

        [HttpPatch("{bookingId}/tender")]
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
