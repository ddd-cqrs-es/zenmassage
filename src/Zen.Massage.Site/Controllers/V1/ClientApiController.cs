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
    /// Client API endpoint
    /// </summary>
    /// <remarks>
    /// Endpoint for interacting with massage clients
    /// and for clients to update their profiles and other client centric settings.
    /// </remarks>
    [Route("api/v1/clients")]
    public class ClientApiControllerV1 : Controller
    {
        [HttpGet]
        [Route("")]
        [SwaggerOperation("GetAll")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IActionResult GetAll()
        {
            return Ok();
        }
    }
}
