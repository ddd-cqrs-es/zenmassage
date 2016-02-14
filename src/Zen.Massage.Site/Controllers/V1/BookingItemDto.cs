using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Zen.Massage.Domain.BookingContext;

namespace Zen.Massage.Site.Controllers.V1
{
    public class BookingItemDto
    {
        [JsonProperty("bookingId")]
        public Guid BookingId { get; set; }

        [JsonProperty("clientId")]
        public Guid ClientId { get; set; }

        [JsonProperty("statusId")]
        public BookingStatus Status { get; set; }

        [JsonProperty("proposedTimeId")]
        public DateTime ProposedTime { get; set; }

        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }

        [JsonProperty("therapistBookings")]
        public ICollection<TherapistBookingItemDto> TherapistBookings { get; set; }
    }

    public class TherapistBookingItemDto
    {
        [JsonProperty("therapistId")]
        public Guid TherapistId { get; set; }

        [JsonProperty("status")]
        public BookingStatus Status { get; set; }

        [JsonProperty("proposedTime")]
        public DateTime ProposedTime { get; set; }
    }
}