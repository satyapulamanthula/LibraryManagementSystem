using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.IServices
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<IEnumerable<BooksCategory>> GetBookCategories();
        //IEnumerable<Semesters> GetSemestersData();
        Task CreateBook(BookModel book);
        Task CreateCategory(BookCategories bookCategories);
        //Task CreateBook(BookModel book);
        //Task CreateCategory(BookCategories bookCategories);
        //Task<IEnumerable<Book>> GetAllBooks();
        //Task<IEnumerable<BooksCategory>> GetBookCategories();
    }
}
