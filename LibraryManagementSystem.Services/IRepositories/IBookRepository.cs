using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Repository.IRepositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooks();
        Task<List<BooksCategory>> GetAllBookCatedgories(); 
        Task CreateBook(BookModel book);
        Task CreateBookCategory(BookCategories bookCategories);
    }
}
