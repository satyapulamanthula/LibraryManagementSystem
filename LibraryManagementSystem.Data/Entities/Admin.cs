using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Entities
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        public string AdminName { get; set; }
        public string Password { get; set; }
        public string Designation { get; set; }
    }
}
