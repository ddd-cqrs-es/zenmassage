using Microsoft.AspNet.Mvc;
using Zen.Massage.Site.Models;

namespace Zen.Massage.Site.Controllers
{
    public class PebbleController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var model =
                new BaseVm
                {
                    Title = "Application Setup"
                };
            return View(model);
        }
    }
}
