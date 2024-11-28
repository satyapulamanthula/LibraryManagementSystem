using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.IServices
{
    public interface IIssuedBookService
    {
        bool IssuedBook(Issued issued);
        IEnumerable<IssuedModel> GetAllIssuedBooks();
        IEnumerable<StudentEnrolment> GetAllStudentsDetails();
        IEnumerable<Book> GetAllBooksDetails();
    }
}
