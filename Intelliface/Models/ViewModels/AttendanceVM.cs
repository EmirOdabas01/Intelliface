namespace Intelliface.Models.ViewModels
{
    public class AttendanceVM
    {
        public DateTime attendanceDate { get; set; }
        public DateTime? checkIn { get; set; }
        public DateTime? checkOut { get; set; }
        public int employeeId { get; set; }
    }
}
