using Intelliface.Models.Dtos;
using Intelliface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
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
            respone.EnsureSuccessStatusCode();
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
            if (!ModelState.IsValid)
                return Redirect("Create");

            var json = JsonSerializer.Serialize(department);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/Department/Create", content);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Records");
        }

        public async Task<IActionResult> Create()
        {
            var response = await _httpClient.GetAsync("/api/Location/GetAllNotSelected");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var locations = JsonSerializer.Deserialize<List<ReadVM<LocationVM>>>(json);

            var model = new LocationDto
            {
                Locations = locations.Select(loc => new SelectListItem
                {
                    Value = loc.id.ToString(),
                    Text = loc.data.address
                }).ToList() ?? new List<SelectListItem> { }
            };

            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var locResponse = await _httpClient.GetAsync($"/api/Department/GetById/{id}");
            locResponse.EnsureSuccessStatusCode();

            var locJson = await locResponse.Content.ReadAsStringAsync();
            var department = JsonSerializer.Deserialize<ReadVM<DepartmentVM>>(locJson);

            var response = await _httpClient.GetAsync("/api/Location/GetAllNotSelected");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var locations = JsonSerializer.Deserialize<List<ReadVM<LocationVM>>>(json);

            var currentLocationResponse = await _httpClient.GetAsync($"/api/Location/GetById/{department?.data.locationId}");
            currentLocationResponse.EnsureSuccessStatusCode();
            var currentLocationJson = await currentLocationResponse.Content.ReadAsStringAsync();

            var currentLocationData = JsonSerializer.Deserialize<ReadVM<LocationVM>>(currentLocationJson);

            locations.Add(new ReadVM<LocationVM>()
            {
                id = department.data.locationId,
                data = new LocationVM
                {
                    address = currentLocationData.data.address,
                    latitude = currentLocationData.data.latitude,
                    longitude = currentLocationData.data.longitude
                }
            });

            var model = new LocationDto
            {
                Locations = locations.Select(loc => new SelectListItem
                {
                    Value = loc.id.ToString(),
                    Text = loc.data.address
                }).ToList() ?? new List<SelectListItem> { }
            };

            return View((model, department));
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentVM department)
        {
            if (!ModelState.IsValid)
                return Redirect("Update");

            var json = JsonSerializer.Serialize(department);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/Department/Update/{id}", content);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Records");

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Department/Delete/{id}");
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Records");
        }
    }
}
