using System;
using Newtonsoft.Json;

namespace Zen.Massage.Site.Controllers.V1
{
    /// <summary>
    /// PlaceBookingBid represents a booking bid request.
    /// </summary>
    public class PlaceBookingBidDto
    {
        /// <summary>
        /// Desired appointment start time (optional)
        /// </summary>
        [JsonProperty("proposedTime")]
        public DateTime? ProposedTime { get; set; }
    }
}