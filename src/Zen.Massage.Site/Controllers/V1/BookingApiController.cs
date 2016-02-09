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
        private readonly IMapper _mapper;

        public BookingApiController(
            MapperConfiguration mapperConfiguration)
        {
            _mapper = mapperConfiguration.CreateMapper();
        }

        [HttpGet("user/{userId}")]
        public Task<IActionResult> GetUserBookings(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
