using Microsoft.AspNetCore.Mvc;

namespace Intelliface.Controllers
{
    public class AttendanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
