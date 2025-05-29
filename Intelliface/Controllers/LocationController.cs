using Intelliface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Intelliface.Controllers
{
    public class LocationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LocationController> _Ilogger;

        public LocationController(ILogger<LocationController> logger, IHttpClientFactory httpClientFactory)
        {
            _Ilogger = logger;
            _httpClient = httpClientFactory.CreateClient("IntellifaceClient");
        }
        public async Task<IActionResult> Records()
        {

            var respone = await _httpClient.GetAsync("/api/Location/GetAll");
            var json = await respone.Content.ReadAsStringAsync();

            var locations = JsonSerializer.Deserialize<List<ReadVM<LocationVM>>>(json);

            return View(locations);
        }

        public async Task<IActionResult> Create() => View();



        [HttpPost]
        public async Task<IActionResult> Create(LocationVM location)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("Create");
            }
            var json = JsonSerializer.Serialize(location);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Location/Create", content);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Records");
        }

        public async Task<IActionResult> Update(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Location/GetById/{id}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var location = JsonSerializer.Deserialize<ReadVM<LocationVM>>(json);
            return View(location);
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] LocationVM location)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("Update");
            }
            var json = JsonSerializer.Serialize(location);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/api/Location/Update/{id}", content);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Records");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Location/Delete/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Records");
        }
    }
}
