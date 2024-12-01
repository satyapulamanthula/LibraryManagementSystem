using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.SharedModels.ViewModels
{
    public class StudentBookViewModel
    {
        public StudentEnrolmentModel? student { get; set; }
        public BookModel? book { get; set; }
        public BookCategories? bookCategories { get; set; }
        public IssuedModel? issued { get; set; }
    }
}
