using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.SharedModels.Models
{
    public class Regester
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password),ErrorMessage ="Password and ConfirmPassword are not Same")]
        public string ConfirmPassword { get;set; }

        //[Required]
        //public string Role { get; set; }
    }
}
