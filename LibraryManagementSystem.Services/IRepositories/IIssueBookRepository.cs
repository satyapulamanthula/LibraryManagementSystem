using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
