using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Mvc;

namespace Zen.Massage.Site.Controllers.V1
{
    [Route("api/bookings")]
    public class BookingApiController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingApiController(
            IBookingService bookingService,
            MapperConfiguration mapperConfiguration)
        {
            _bookingService = bookingService;
            _mapper = mapperConfiguration.CreateMapper();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserBookings(Guid userId)
        {
            var bookings = await _bookingService.GetByUserId(userId).ConfigureAwait(true);
            var bookingItems = _mapper.Map<ICollection<BookingItemDto>>(bookings);
            var result = 
                new ResultDto<ICollection<BookingItemDto>>
                {
                    StatusCode = 200,
                    StatusDescription = "OK",
                    Result = bookingItems
                };
            return Json(result);
        }
    }
}
