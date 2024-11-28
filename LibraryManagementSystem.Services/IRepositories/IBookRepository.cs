using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Repository.IRepositories
{
    public interface IBookRepository
    {
        //List<Book> GetAllBooks();
        Task<List<Book>> GetAllBooks();
        Task<List<BooksCategory>> GetAllBookCatedgories(); 
        //List<Semesters> GetAllSemesters();
        Task CreateBook(BookModel book);
        Task CreateBookCategory(BookCategories bookCategories);
    }
}
