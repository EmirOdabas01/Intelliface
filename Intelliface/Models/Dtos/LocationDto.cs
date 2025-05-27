using Microsoft.AspNetCore.Mvc.Rendering;

namespace Intelliface.Models.Dtos
{
    public class LocationDto
    {
       public int Id { get; set; }
       public List<SelectListItem> Locations { get; set; }
    }
}
