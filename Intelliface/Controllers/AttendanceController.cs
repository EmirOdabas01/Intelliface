using Intelliface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Intelliface.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AttendanceController> _Ilogger;

        public AttendanceController(ILogger<AttendanceController> logger, IHttpClientFactory httpClientFactory)
        {
            _Ilogger = logger;
            _httpClient = httpClientFactory.CreateClient("IntellifaceClient");
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Attendance/GetAttendancesByEmployeeId/{id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var attendanceRecords = JsonSerializer.Deserialize<List<ReadVM<AttendanceVM>>>(json);

            return View(attendanceRecords);
        }
    }
}
