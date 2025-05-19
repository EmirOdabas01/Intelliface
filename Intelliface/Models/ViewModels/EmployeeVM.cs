namespace Intelliface.Models.ViewModels
{
    public class EmployeeVM
    {
        public string name { get; set; } = String.Empty;
        public string surname { get; set; } = String.Empty;
        public string phoneNumber { get; set; } = String.Empty;
        public string email { get; set; } = String.Empty;
        public string password { get; set; } = String.Empty;
        public int departmentId { get; set; }
        public bool isAdmin { get; set; }
    }
}
