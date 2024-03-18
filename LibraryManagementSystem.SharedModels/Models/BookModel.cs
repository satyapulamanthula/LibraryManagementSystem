using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.SharedModels.Models
{
    public class BookModel
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Book Name is required.")]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Author Name is required.")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Publishing is required.")]
        public string Publishing { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Subject type is required.")]
        public string Subject { get; set; }
    }
}
