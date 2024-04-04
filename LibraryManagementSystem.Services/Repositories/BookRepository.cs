using LibraryManagementSystem.SharedModels.Models;
using LibraryManagementSystem.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data.Entities;
using System.Diagnostics;
using System.Xml.Linq;

namespace LibraryManagementSystem.Repository.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _dbContext;
        private readonly ILibraryManagementLogger _logger;

        public BookRepository(LibraryDbContext dbContext, ILibraryManagementLogger libraryManagementLogger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = libraryManagementLogger ?? throw new ArgumentNullException(nameof(libraryManagementLogger));
        }
        public List<Book> GetAllBooks()
        {
            return _dbContext.Books.ToList();
        }

        public List<BooksCategory> GetAllBookCatedgories()
        {
            return _dbContext.BookCategories.ToList();
        }

        //public List<Semesters> GetAllSemesters()
        //{
        //    return _dbContext.Semesterss.ToList();
        //}

        public void CreateBook(BookModel book)
        {
            try
            {
                if (book == null)
                {
                    _logger.LogWarning("Book Details are empty");
                }
                else
                {
                    var bookEntity = new Book
                    {
                        BookId = book.BookId,
                        BookName = book.BookName,
                        AuthorName = book.AuthorName,
                        Publishing = book.Publishing,
                        Price = book.Price,
                        Subject = book.Subject,
                        //Semester = book.SemesterId
                    };

                    _dbContext.Books.Add(bookEntity);
                    _dbContext.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add a book data with Exception: {ex.StackTrace}");
                throw;
            }
        }

        public void CreateBookCategory(BookCategories category)
        {
            try
            {
                if (category == null)
                {
                    _logger.LogWarning("Book Category is empty");
                    return;
                }

                // Checking that if a category with the same subject already exists or not
                var existingCategory = _dbContext.BookCategories.FirstOrDefault(c => c.Subject == category.Subject);

                if (existingCategory != null)
                {
                    _logger.LogWarning($"A category with the subject '{category.Subject}' already exists.");
                    return;
                }

                var newCategory = new BooksCategory
                {
                    Subject = category.Subject,
                };

                _dbContext.BookCategories.Add(newCategory); // Add the new category
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add a category with Exception: {ex.StackTrace}");
                throw;
            }
        }
    }
}
