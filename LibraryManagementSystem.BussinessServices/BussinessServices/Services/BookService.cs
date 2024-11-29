using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository BookRepository;
        public BookService(IBookRepository bookRepository)
        {
            BookRepository = bookRepository;
        }

        public async Task CreateBook(BookModel book)
        {
            await BookRepository.CreateBook(book);
        }

        public async Task CreateCategory(BookCategories bookCategories)
        {
            await BookRepository.CreateBookCategory(bookCategories);
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await BookRepository.GetAllBooks();
        }
        public async Task<IEnumerable<BooksCategory>> GetBookCategories()
        {
            return await BookRepository.GetAllBookCatedgories();
        }

        //public IEnumerable<Semesters> GetSemestersData()
        //{
        //    return BookRepository.GetAllSemesters();
        //}
    }
}
