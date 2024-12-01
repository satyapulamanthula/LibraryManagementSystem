using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Data.Entities
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        public string? AdminName { get; set; }
        public string? Password { get; set; }
        public string? Designation { get; set; }
    }
}
