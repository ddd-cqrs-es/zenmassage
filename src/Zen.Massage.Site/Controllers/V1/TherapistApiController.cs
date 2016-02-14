using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Zen.Massage.Site.Controllers.V1
{
    /// <summary>
    /// Therapist API endpoint
    /// </summary>
    /// <remarks>
    /// Endpoint for interacting with therapists
    /// Used to setup therapist home locality, skillset and charge band
    /// </remarks>
    [Route("api/v1/users")]
    public class TherapistApiControllerV1 : Controller
    {
        [HttpGet]
        [Route("")]
        [SwaggerOperation("GetAllUsers")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}
