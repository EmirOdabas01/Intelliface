using Microsoft.AspNetCore.Mvc;

namespace Intelliface.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
