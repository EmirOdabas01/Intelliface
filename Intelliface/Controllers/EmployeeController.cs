using Intelliface.Models.Dtos;
using Intelliface.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace Intelliface.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmployeeController> _Ilogger;

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

            List<string> departmentList = new List<string>();


            var depResponse = await _httpClient.GetAsync("api/Department/GetAll");
            var depJson = await depResponse.Content.ReadAsStringAsync();

            var departments = JsonSerializer.Deserialize<List<ReadVM<DepartmentVM>>>(depJson);

            for (int i = 0; i < employees.Count; i++)
            {
                var x = departments.FirstOrDefault(dep => dep.id == employees[i].data.departmentId);

                departmentList.Add(x.data.name);
            }

            return View((employees, departmentList));
        }
        public async Task<IActionResult> Create()
        {

            var response = await _httpClient.GetAsync("/api/Department/GetAll");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var departments = JsonSerializer.Deserialize<List<ReadVM<DepartmentVM>>>(json);

            var model = new DepartmentDto
            {
                Departments = departments?.Select(dep => new SelectListItem
                {
                    Value = dep.id.ToString(),
                    Text = dep.data.name
                }).ToList() ?? new List<SelectListItem>()
            };

            return View((model, new ReadVM<EmployeeVM> { }));
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeVM employee)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("Create");
            }

            var json = JsonSerializer.Serialize(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/Employee/Create", content);
            response.EnsureSuccessStatusCode();

            return Redirect("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Update(EmployeeVM employee, int? id)
        {

            var json = JsonSerializer.Serialize(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/api/Employee/Update/{id}", content);
            response.EnsureSuccessStatusCode();

            return Redirect("Records");
        }
        public async Task<IActionResult> Update(int? EmployeeId)
        {
            ReadVM<EmployeeVM> employee = null;

            if (EmployeeId.HasValue)
            {
                var empResponse = await _httpClient.GetAsync($"/api/Employee/GetById/{EmployeeId}");
                if (empResponse.IsSuccessStatusCode)
                {
                    var empJson = await empResponse.Content.ReadAsStringAsync();
                    employee = JsonSerializer.Deserialize<ReadVM<EmployeeVM>>(empJson);
                }
                else
                {
                    employee = null;
                }
            }

            var response = await _httpClient.GetAsync("/api/Department/GetAll");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var departments = JsonSerializer.Deserialize<List<ReadVM<DepartmentVM>>>(json);

            var model = new DepartmentDto
            {
                Departments = departments?.Select(dep => new SelectListItem
                {
                    Value = dep.id.ToString(),
                    Text = dep.data.name
                }).ToList() ?? new List<SelectListItem>()
            };

            return View("Create", (model, employee));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Employee/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                return Ok(); 
            }

            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }


        
    }
}
