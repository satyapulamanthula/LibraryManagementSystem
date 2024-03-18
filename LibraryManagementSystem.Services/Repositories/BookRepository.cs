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
        //private readonly LibraryDbContext _dbContext;

        //private ILibraryManagementLogger _logger;
        //public BookRepository(LibraryDbContext dbContext,ILibraryManagementLogger libraryManagementLogger)
        //{
        //    _dbContext = dbContext;
        //    _logger = libraryManagementLogger;
        //}

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
                        Subject = book.Subject 
                    };

                    _dbContext.Books.Add(bookEntity);
                    _dbContext.SaveChanges();

                    // After SaveChanges, the book entity will be updated with the generated BookId
                    int savedBookId = book.BookId;

                    _logger.LogInformation($"Book (ID: {savedBookId}) is added successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add a book data with Exception: {ex.StackTrace}");
                throw;
            }
        }

        //public void CreateBookCategory(BookCategories category)
        //{
        //    try
        //    {
        //        if (category == null)
        //        {
        //            _logger.LogWarning("Book Category is empty");
        //        }
        //        else
        //        {
        //            // Check if a category with the same subject already exists
        //            var existingCategory = _dbContext.BookCategories.FirstOrDefault(c => c.Subject == category.Subject);

        //            if (existingCategory != null)
        //            {
        //                _logger.LogWarning($"A category with the subject '{category.Subject}' already exists.");
        //                return; // Exit the method without saving
        //            }

        //            _dbContext.BookCategories.Add(category);
        //            _dbContext.SaveChanges();

        //            // After SaveChanges, the category entity will be updated with the generated BookCategoryId
        //            int savedCategoryId = category.BookCategoryId;

        //            _logger.LogInformation($"Category (ID: {savedCategoryId}) is added successfully");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Failed to add a category with Exception: {ex.StackTrace}");
        //        throw;
        //    }
        //}


        // Modified CreateBookCategory method to accept LibraryManagementSystem.SharedModels.Models.BookCategories
        public void CreateBookCategory(BookCategories category)
        {
            try
            {
                if (category == null)
                {
                    _logger.LogWarning("Book Category is empty");
                    return;
                }

                // Check if a category with the same subject already exists
                var existingCategory = _dbContext.BookCategories.FirstOrDefault(c => c.Subject == category.Subject);

                if (existingCategory != null)
                {
                    _logger.LogWarning($"A category with the subject '{category.Subject}' already exists.");
                    return; // Exit the method without saving
                }

                // Convert LibraryManagementSystem.SharedModels.Models.BookCategories to LibraryManagementSystem.Data.Entities.BooksCategory
                var newCategory = new BooksCategory
                {
                    Subject = category.Subject,
                    // Assign other properties if needed
                };

                _dbContext.BookCategories.Add(newCategory); // Add the new category
                _dbContext.SaveChanges();

                // After SaveChanges, the category entity will be updated with the generated BookCategoryId
                int savedCategoryId = newCategory.BookCategoryId;

                _logger.LogInformation($"Category (ID: {savedCategoryId}) is added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add a category with Exception: {ex.StackTrace}");
                throw;
            }
        }

        public object GetAllStudents()
        {
            throw new NotImplementedException();
        }







        //public void GetAllRoles()
        //{
        //    var result = from u in _dbContext.AspNetUsers
        //        left join ur in _dbContext.AspNetUserRoles
        //        left join r in _dbContext.AspNetRoles
        //        select new
        //        {
        //            UserId = u.Id,
        //            UserName = u.UserName,
        //            RoleId = r.Id,
        //            RoleName = r.Name
        //        };
        //    return result;
        //}

    }
}
