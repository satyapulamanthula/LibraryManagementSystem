using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Repository.Repositories
{
    public class IssueBookRepository : IIssueBookRepository
    {
        private LibraryDbContext DbContext;
        private ILogger<IssueBookRepository> Logger;
        public IssueBookRepository(LibraryDbContext dbContext, ILogger<IssueBookRepository> libraryManagementLogger)
        {
            DbContext = dbContext;
            Logger = libraryManagementLogger;
        }
        public bool IssuedBook(Issued issued)
        {
            try
            {
                if (issued == null)
                {
                    Logger.LogWarning("IssuedBook Details are empty");
                }
                else
                {
                    // Check if the student has already issued three books and book is active or not 
                    //int studentId = issued.StudentId;
                    //int bookId = issued.BookId;
                    int issuedBooksCount = DbContext.Issued
                        .Count(i => i.StudentId == issued.StudentId && i.IsReturn == false);

                    if (issuedBooksCount >= 3)
                    {
                        // Student has already issued three books
                        Logger.LogWarning($"Student with ID {issued.StudentId} has already issued three books.");
                        return false;
                    }

                    bool issuedBook = DbContext.Issued.Any(i => i.BookId == issued.BookId && i.IsReturn == true);
                    if (issuedBook == true)
                    {
                        // Book has already issued
                        Logger.LogWarning($"This book is already issued to someone");
                        return false;
                    }

                    // Continue with the issuance process
                    DbContext.Issued.Add(issued);
                    DbContext.SaveChanges();

                    // After SaveChanges, the book entity will be updated with the generated BookId
                    int issuedBookId = issued.IssueId;

                    Logger.LogInformation($"Book (ID: {issuedBookId}) is added successfully");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to add a book data with Exception: {ex.StackTrace}");
                throw;
            }
            return true;
        }

        public List<IssuedModel> GetAllIssuedBooks()
        {
            //// Ignore it For my reference.
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

            var issuedBooks = DbContext.Issued
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
            return DbContext.StudentEnrolments.ToList();
        }

        public List<Book> GetAllBooks()
        {
            return DbContext.Books.ToList();
        }

    }
}
