using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Zen.Massage.Domain.BookingBoundedContext;
using Zen.Massage.Domain.BookingContext;
using Zen.Massage.Site.Controllers.V1;

namespace Zen.Massage.Site
{
    public class AutomapperConfigProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<IReadBooking, BookingItemDto>();
            CreateMap<IReadTherapistBooking, TherapistBookingItemDto>();

            CreateMap<IBooking, BookingItemDto>();
            CreateMap<ITherapistBooking, TherapistBookingItemDto>();
        }
    }
}
