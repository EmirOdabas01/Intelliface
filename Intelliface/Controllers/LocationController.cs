using Microsoft.AspNetCore.Mvc;

namespace Intelliface.Controllers
{
    public class LocationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
