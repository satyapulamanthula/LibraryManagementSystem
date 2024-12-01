using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Data.Entities
{
    public class StudentEnrolment
    {
        [Key]
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? Department { get; set; }
        public string? Semester { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? StudentContact { get; set; }
        public string? StudentEmail { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
