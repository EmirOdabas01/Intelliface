using Microsoft.AspNetCore.Mvc.Rendering;

namespace Intelliface.Models.Dtos
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public List<SelectListItem> Departments { get; set; }
    }
}
