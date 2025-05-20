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

            List<string> locations = new List<string>();

            for(int i = 0; i < departments.Count; i++)
            {
                var locationResponse = await _httpClient.GetAsync("/api/Location/GetById/" + departments[i].data.locationId.ToString());
                var locationjson = await locationResponse.Content.ReadAsStringAsync();

                var location = JsonSerializer.Deserialize<LocationVM>(locationjson);

                locations.Add(location.address);
            }

            return View((departments, locations));
        }
    }
}
