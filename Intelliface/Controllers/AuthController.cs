using Intelliface.Models;
using Intelliface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;

namespace Intelliface.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly HttpClient _httpClient;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5206")  
            };
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM user)
        {
            if (!ModelState.IsValid)
            {
                TempData["EMailError"] = ModelState["EMail"]?.Errors.FirstOrDefault()?.ErrorMessage;
                TempData["PasswordError"] = ModelState["Password"]?.Errors.FirstOrDefault()?.ErrorMessage;

                return View(user);
            }

            var response = await _httpClient.PostAsJsonAsync("/api/Employee/IsAdmin", user);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["LoginError"] = "Giriş başarısız. Bilgileri kontrol edin.";
                return View(user);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
