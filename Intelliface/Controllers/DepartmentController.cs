using Microsoft.AspNetCore.Mvc;

namespace Intelliface.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
