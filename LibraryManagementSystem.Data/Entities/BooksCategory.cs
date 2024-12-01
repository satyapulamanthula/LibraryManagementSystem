using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Data.Entities
{
    public class BooksCategory
    {
        [Key]
        public int BookCategoryId { get; set; }

        public string? Subject { get; set; }

        ////public string CategoryName { get; set; }
        //public string Description { get; set; }

        //// Navigation property
        //public ICollection<Book> Books { get; set; }
    }
}
