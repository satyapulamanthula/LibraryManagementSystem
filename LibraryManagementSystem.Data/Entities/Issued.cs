using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Data.Entities
{
    public class Issued
    {
        [Key]
        public int IssueId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        public  StudentEnrolment Student { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime? ReturnedDate { get; set; }
        public bool IsReturn { get; set; }
        public int Days { get; set; }
        public bool IsFinePaid { get; set; }
        public decimal FineAmount { get; set; }

    }
}
