using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository.Repositories
{
    public class ReturnBookRepository : IReturnBookRepository
    {
        private readonly LibraryDbContext DbContext;
        private readonly ILibraryManagementLogger Logger;

        public ReturnBookRepository(LibraryDbContext dbContext, ILibraryManagementLogger libraryManagementLogger)
        {
            DbContext = dbContext;
            Logger = libraryManagementLogger;
        }

        public async Task<List<IssuedModel>> GetAllIssuedBooks(int studentId, bool isReturn, bool isFinePaid)
        {
            ////// Ignoreit For my reference.
            ////var issuedBooksForStudent = (from i in DbContext.Issued
            ////                             join se in DbContext.StudentEnrolments on i.StudentId equals se.StudentId
            ////                             join b in DbContext.Books on i.BookId equals b.BookId
            ////                             where i.StudentId == studentId && i.IsReturn == false
            ////                             select new IssuedModel
            ////                             {
            ////                                 IssueId = i.IssueId,
            ////                                 StudentId = i.StudentId,
            ////                                 StudentName = se.StudentName,
            ////                                 BookId = i.BookId,
            ////                                 BookName = b.BookName,
            ////                                 IssueDate = i.IssueDate,
            ////                                 IsReturn = i.IsReturn
            ////                             }).ToList();

            var issuedBooksForStudent = await DbContext.Issued
                .Include(x => x.Student)
                .Include(x => x.Book)
                .Where(i => i.StudentId == studentId && i.IsReturn == isReturn && i.IsFinePaid == isFinePaid)
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
                }).ToListAsync();

            return issuedBooksForStudent;

            //try
            //{
            //    // Directly query IssuedModel using raw SQL
            //    var issuedBooks = await DbContext.Set<IssuedModel>()
            //        .FromSqlInterpolated($@"
            //    EXEC GetAllIssuedBooks 
            //    @StudentId = {studentId}, 
            //    @IsReturn = {isReturn}, 
            //    @IsFinePaid = {isFinePaid}")
            //        .ToListAsync();

            //    return issuedBooks;
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogError($"Error while fetching issued books: {ex.Message}");
            //    throw;
            //}
        }

        public async Task UpdatingBook(List<int> issuedBookIds)
        {
            foreach (var issuedBookId in issuedBookIds)
            {
                var issueToUpdate = await DbContext.Issued.SingleOrDefaultAsync(issue => issue.IssueId == issuedBookId);

                if (issueToUpdate != null)
                {
                    issueToUpdate.IsReturn = true;
                    issueToUpdate.ReturnedDate = DateTime.Now;

                    // Calculate the days between IssueDate and ReturnedDate
                    var daysDifference = (issueToUpdate.ReturnedDate.Value - issueToUpdate.IssueDate).Days;
                    issueToUpdate.Days = daysDifference;

                    // Determine FineAmount based on daysDifference
                    if (daysDifference <= 30)
                    {
                        issueToUpdate.FineAmount = 0;
                        issueToUpdate.IsFinePaid = true;
                    }
                    else
                    {
                        // Calculate the fine for each 30-day period beyond the first 30 days
                        int extraPeriods = (daysDifference - 30) / 30;
                        issueToUpdate.FineAmount = 10 * (extraPeriods + 1);
                        issueToUpdate.IsFinePaid = false;
                    }
                }
            }
            await DbContext.SaveChangesAsync();
        }


        //// Ignoer it.
        //public void UpdatingBook(List<int> issuedBookIds)
        //{
        //    foreach (var issuedBookId in issuedBookIds)
        //    {
        //        var issueToUpdate = DbContext.Issued.SingleOrDefault(issue => issue.IssueId == issuedBookId);

        //        if (issueToUpdate != null)
        //        {
        //            issueToUpdate.IsReturn = true;
        //            issueToUpdate.ReturnedDate = DateTime.Now;

        //            // Calculating the days between StartDate and ReturnedDate and checking the given date
        //            var daysDifference = (issueToUpdate.ReturnedDate.Value - issueToUpdate.IssueDate).Days;
        //            issueToUpdate.Days = daysDifference;
        //            if (daysDifference <= 30)
        //            {
        //                issueToUpdate.FineAmount = 0;
        //                issueToUpdate.IsFinePaid = true;
        //            }
        //            else if (daysDifference <= 60)
        //            {
        //                issueToUpdate.FineAmount = 10;
        //                issueToUpdate.IsFinePaid = false;
        //            }
        //            else if (daysDifference <= 90)
        //            {
        //                issueToUpdate.FineAmount = 20;
        //                issueToUpdate.IsFinePaid = false;
        //            }
        //            else
        //            {
        //                issueToUpdate.FineAmount = 30;
        //                issueToUpdate.IsFinePaid = false;
        //            }
        //        }
        //    }

        //    DbContext.SaveChanges();
        //}

        public async Task UpdatingFine(List<int> issuedBookIds)
        {
            foreach(var issuedBookId in issuedBookIds)
            {
                var fineToUpdate = await DbContext.Issued.SingleOrDefaultAsync(issue => issue.IssueId == issuedBookId);
                if(fineToUpdate != null)
                {
                    fineToUpdate.IsFinePaid = true;
                }
                await DbContext.SaveChangesAsync();
            }
        } 

        //public List<Issued> GetAllIssuedBooks()
        //{
        //    return DbContext.Issued.ToList();
        //}
    }
}
