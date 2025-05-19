using Intelliface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Intelliface.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        ILogger<EmployeeController> _Ilogger;

        public EmployeeController(ILogger<EmployeeController> logger, IHttpClientFactory httpClientFactory)
        {
            _Ilogger = logger;
            _httpClient = httpClientFactory.CreateClient("IntellifaceClient");
        }
        public async Task<IActionResult> Records()
        {
            var response = await _httpClient.GetAsync("/api/Employee/GetAll");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var employees = JsonSerializer.Deserialize<List<ReadVM<EmployeeVM>>>(json); 

            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
