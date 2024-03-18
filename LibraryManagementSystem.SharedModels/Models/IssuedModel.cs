using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.SharedModels.Models
{
    public class IssuedModel
    {
        public int IssueId { get; set; }

        [Required(ErrorMessage = " StudentId is required.")]
        public int StudentId { get; set; }

        public string StudentName { get; set; }

        [Required(ErrorMessage = " BookId is required.")]
        public int BookId { get; set; }

        public string BookName { get; set; }

        [Required(ErrorMessage = " IssueDate is required.")]
        public DateTime IssueDate { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public bool IsReturn { get; set; }

        public int Days { get; set; }

        public decimal FineAmount { get; set; }

        public bool IsFinePaid { get; set; }
    }
}
