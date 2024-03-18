using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Repository.Repositories
{
    public class IssueBookRepository : IIssueBookRepository
    {
        private LibraryDbContext _dbContext;
        private ILibraryManagementLogger _logger;
        public IssueBookRepository(LibraryDbContext dbContext, ILibraryManagementLogger libraryManagementLogger)
        {
            _dbContext = dbContext;
            _logger = libraryManagementLogger;
        }
        public bool IssuedBook(Issued issued)
        {
            try
            {
                if (issued == null)
                {
                    _logger.LogWarning("IssuedBook Details are empty");
                }
                else
                {
                    // Check if the student has already issued three books and book is active or not 
                    int studentId = issued.StudentId;
                    int bookId = issued.BookId;
                    int issuedBooksCount = _dbContext.Issued
                        .Count(i => i.StudentId == studentId && i.IsReturn == false);

                    if (issuedBooksCount >= 3)
                    {
                        // Student has already issued three books
                        _logger.LogWarning($"Student with ID {studentId} has already issued three books.");
                        return false;
                    }

                    //bool issuedBook = _dbContext.Issued.Any(i => i.BookId == bookId && i.IsReturn == true);
                    //if (issuedBook = true)
                    //{
                    //    // Book has already issued
                    //    _logger.LogWarning($"This book is already issued to someone");
                    //    // You can throw an exception or return an appropriate response here
                    //    // Depending on your application logic
                    //    return;
                    //}

                    // Continue with the issuance process
                    _dbContext.Issued.Add(issued);
                    _dbContext.SaveChanges();

                    // After SaveChanges, the book entity will be updated with the generated BookId
                    int issuedBookId = issued.IssueId;

                    _logger.LogInformation($"Book (ID: {issuedBookId}) is added successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add a book data with Exception: {ex.StackTrace}");
                throw;
            }
            return true;
        }

        public List<IssuedModel> GetAllIssuedBooks()
        {
            //var issuedBooks = (from i in _dbContext.Issued
            //                   join se in _dbContext.StudentEnrolments on i.StudentId equals se.StudentId
            //                   join b in _dbContext.Books on i.BookId equals b.BookId
            //                   select new IssuedModel
            //                   {
            //                       IssueId = i.IssueId,
            //                       StudentId = i.StudentId,
            //                       StudentName = se.StudentName,
            //                       BookId = i.BookId,
            //                       BookName = b.BookName,
            //                       IssueDate = i.IssueDate,
            //                       IsReturn = i.IsReturn
            //                   }).ToList();

            //return issuedBooks;

            var issuedBooks = _dbContext.Issued
            .Include(i => i.Student)
            .Include(i => i.Book)
            .Select(i => new IssuedModel
            {
                IssueId = i.IssueId,
                StudentId = i.StudentId,
                StudentName = i.Student.StudentName,
                BookId = i.BookId,
                BookName = i.Book.BookName,
                IssueDate = i.IssueDate,
                IsReturn = i.IsReturn,
                ReturnedDate = i.ReturnedDate,
                Days = i.Days,
                FineAmount = i.FineAmount,
                IsFinePaid = i.IsFinePaid
            })
            .ToList();
            return issuedBooks;
        }

        public List<StudentEnrolment> GetAllStudents()
        {
            return _dbContext.StudentEnrolments.ToList();
        }

        public List<Book> GetAllBooks()
        {
            return _dbContext.Books.ToList();
        }

    }
}
