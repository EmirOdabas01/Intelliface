using Intelliface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Intelliface.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DepartmentController> _Ilogger;

        public DepartmentController(ILogger<DepartmentController> logger, IHttpClientFactory httpClientFactory)
        {
            _Ilogger = logger;
            _httpClient = httpClientFactory.CreateClient("IntellifaceClient");
        }
        public async Task<IActionResult> Records()
        {
            var respone = await _httpClient.GetAsync("/api/Department/GetAll");
            var json = await respone.Content.ReadAsStringAsync();

            var departments = JsonSerializer.Deserialize<List<ReadVM<DepartmentVM>>>(json);

            List<string> locationList = new List<string>();


            var locResponse = await _httpClient.GetAsync("api/Location/GetAll");
            var locJson = await locResponse.Content.ReadAsStringAsync();

            var locations = JsonSerializer.Deserialize<List<ReadVM<LocationVM>>>(locJson);

            for (int i = 0; i < departments.Count; i++)
            {
                var x = locations.FirstOrDefault(loc => loc.id == departments[i].data.locationId);

                locationList.Add(x.data.address);
            }

            return View((departments, locationList));
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentVM department)
        {


            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

    }
}
