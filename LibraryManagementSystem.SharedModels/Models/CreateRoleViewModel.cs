using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.SharedModels.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string? RoleName { get; set; }

        //public string RoleId { get; set; }

        //public string UserId { get; set; }

        //public string UserName { get; set; }


    }
}
