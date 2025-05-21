using Intelliface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
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
    }
}
