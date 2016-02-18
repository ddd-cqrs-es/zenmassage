using System;
using Newtonsoft.Json;

namespace Zen.Massage.Site.Controllers.V1
{
    /// <summary>
    /// CreateBooking represents a booking request.
    /// </summary>
    public class CreateBookingDto
    {
        /// <summary>
        /// Desired appointment start time
        /// </summary>
        [JsonProperty("proposedTime")]
        public DateTime ProposedTime { get; set; }

        /// <summary>
        /// Desired treatment duration
        /// </summary>
        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }
    }
}