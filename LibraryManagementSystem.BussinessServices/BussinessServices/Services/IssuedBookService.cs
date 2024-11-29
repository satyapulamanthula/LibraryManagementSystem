using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class IssuedBookService : IIssuedBookService
    {
        public readonly IIssueBookRepository IssueBookRepository;
        public IssuedBookService(IIssueBookRepository issueBookRepository)
        {
            IssueBookRepository = issueBookRepository;
        }

        public bool IssuedBook(Issued issued)
        {
            return IssueBookRepository.IssuedBook(issued);
        }

        public IEnumerable<IssuedModel> GetAllIssuedBooks()
        {
            return IssueBookRepository.GetAllIssuedBooks();
        }

        public IEnumerable<StudentEnrolment> GetAllStudentsDetails()
        {
            return IssueBookRepository.GetAllStudents();
        }

        public IEnumerable<Book>GetAllBooksDetails()
        {
            return IssueBookRepository.GetAllBooks();
        }
    }
}
