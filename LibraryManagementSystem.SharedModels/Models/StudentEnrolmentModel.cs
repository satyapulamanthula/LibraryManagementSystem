using LibraryManagementSystem.SharedModels.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.SharedModels.Models
{
    public class StudentEnrolmentModel
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        public string? StudentName { get; set; }

        [Required(ErrorMessage = "Student Department is required.")]
        public string? Department { get; set; }

        [Required(ErrorMessage = "Semester is required.")]
        public string? Semester { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Student DOB")]
        [MinimumAge(20, ErrorMessage = "The minimum age requirement is 20 years.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Student Contact number is required.")]
        public string? StudentContact { get; set; }

        [Required(ErrorMessage = "Student Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? StudentEmail { get; set; }

        [Required(ErrorMessage = "Start date cannot be empty")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date cannot be empty")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        [DateComparison("StartDate", ErrorMessage = "End Date must be greater than Start Date.")]
        //[CustomComparing(ErrorMessage = "End Date must be greater than Start Date and at least 6 months.")]

        [MinimumDuration(6, ErrorMessage = "The minimum duration between Start Date and End Date is 6 months.")]

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        //public ICollection<IssuedModel> Issues { get; set; }
    }
}
