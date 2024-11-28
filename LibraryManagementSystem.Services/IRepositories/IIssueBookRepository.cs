using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Repository.IRepositories
{
    public interface IIssueBookRepository
    {
        bool IssuedBook(Issued issued);
        List<IssuedModel> GetAllIssuedBooks();
        List<StudentEnrolment> GetAllStudents();
        List<Book> GetAllBooks();

    }
}
