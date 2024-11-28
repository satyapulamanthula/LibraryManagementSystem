using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task CreateBook(BookModel book)
        {
            await _bookRepository.CreateBook(book);
        }

        public async Task CreateCategory(BookCategories bookCategories)
        {
            await _bookRepository.CreateBookCategory(bookCategories);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAllBooks();
        }
        public async Task<IEnumerable<BooksCategory>> GetBookCategories()
        {
            return await _bookRepository.GetAllBookCatedgories();
        }

        //public IEnumerable<Semesters> GetSemestersData()
        //{
        //    return _bookRepository.GetAllSemesters();
        //}
    }
}
