using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagementSystem.Repository.Repositories
{
    public class ReturnBookRepository : IReturnBookRepository
    {
        private readonly LibraryDbContext _dbContext;
        private readonly ILibraryManagementLogger _logger;

        public ReturnBookRepository(LibraryDbContext dbContext, ILibraryManagementLogger libraryManagementLogger)
        {
            _dbContext = dbContext;
            _logger = libraryManagementLogger;
        }

        public List<IssuedModel> GetAllIssuedBooks(int studentId, bool isReturn, bool isFinePaid)
        {
            //var issuedBooksForStudent = (from i in _dbContext.Issued
            //                             join se in _dbContext.StudentEnrolments on i.StudentId equals se.StudentId
            //                             join b in _dbContext.Books on i.BookId equals b.BookId
            //                             where i.StudentId == studentId && i.IsReturn == false
            //                             select new IssuedModel
            //                             {
            //                                 IssueId = i.IssueId,
            //                                 StudentId = i.StudentId,
            //                                 StudentName = se.StudentName,
            //                                 BookId = i.BookId,
            //                                 BookName = b.BookName,
            //                                 IssueDate = i.IssueDate,
            //                                 IsReturn = i.IsReturn
            //                             }).ToList();
            var issuedBooksForStudent = _dbContext.Issued
                .Include(x=> x.Student)
                .Include(x=> x.Book)
                .Where(i=>i.StudentId== studentId && i.IsReturn == isReturn && i.IsFinePaid == isFinePaid)
                .Select(i=>new IssuedModel
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
                }).ToList();

            return issuedBooksForStudent;
        }

        public void UpdatingBook(List<int> issuedBookIds)
        {
            foreach (var issuedBookId in issuedBookIds)
            {
                var issueToUpdate = _dbContext.Issued.SingleOrDefault(issue => issue.IssueId == issuedBookId);

                if (issueToUpdate != null)
                {
                    issueToUpdate.IsReturn = true;
                    issueToUpdate.ReturnedDate = DateTime.Now;

                    // Calculate the difference between StartDate and ReturnedDate
                    var daysDifference = (issueToUpdate.ReturnedDate.Value - issueToUpdate.IssueDate).Days;
                    issueToUpdate.Days = daysDifference;
                    if (daysDifference <= 30)
                    {
                        issueToUpdate.FineAmount = 0;
                        issueToUpdate.IsFinePaid = true;
                    }
                    else if (daysDifference <= 60)
                    {
                        issueToUpdate.FineAmount = 10;
                        issueToUpdate.IsFinePaid = false;
                    }
                    else if (daysDifference <= 90)
                    {
                        issueToUpdate.FineAmount = 20;
                        issueToUpdate.IsFinePaid = false;
                    }
                    else
                    {
                        issueToUpdate.FineAmount = 30; // Or any other amount you need
                        issueToUpdate.IsFinePaid = false;
                    }
                }
            }

            _dbContext.SaveChanges();
        }

        public void UpdatingFine(List<int> issuedBookIds)
        {
            foreach(var issuedBookId in issuedBookIds)
            {
                var fineToUpdate = _dbContext.Issued.SingleOrDefault(issue => issue.IssueId == issuedBookId);
                if(fineToUpdate != null)
                {
                    fineToUpdate.IsFinePaid = true;
                }
                _dbContext.SaveChanges();
            }
        } 


        public List<Issued> GetAllIssuedBooks()
        {
            return _dbContext.Issued.ToList();
        }
    }
}
