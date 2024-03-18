using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class IssuedBookService : IIssuedBookService
    {
        public readonly IIssueBookRepository _issueBookRepository;
        public IssuedBookService(IIssueBookRepository issueBookRepository)
        {
            _issueBookRepository = issueBookRepository;
        }

        public bool IssuedBook(Issued issued)
        {
            return _issueBookRepository.IssuedBook(issued);
        }

        public IEnumerable<IssuedModel> GetAllIssuedBooks()
        {
            return _issueBookRepository.GetAllIssuedBooks();
        }

        public IEnumerable<StudentEnrolment> GetAllStudentsDetails()
        {
            return _issueBookRepository.GetAllStudents();
        }

        public IEnumerable<Book>GetAllBooksDetails()
        {
            return _issueBookRepository.GetAllBooks();
        }
    }
}
