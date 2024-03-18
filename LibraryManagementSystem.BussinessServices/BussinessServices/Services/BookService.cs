using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Data.Entities;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void CreateBook(BookModel book)
        {
            _bookRepository.CreateBook(book);
        }

        public void CreateCategory(BookCategories bookCategories)
        {
            _bookRepository.CreateBookCategory(bookCategories);
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _bookRepository.GetAllBooks();
        }
        public IEnumerable<BooksCategory> GetBookCategories()
        {
            return _bookRepository.GetAllBookCatedgories();
        }
    }
}
