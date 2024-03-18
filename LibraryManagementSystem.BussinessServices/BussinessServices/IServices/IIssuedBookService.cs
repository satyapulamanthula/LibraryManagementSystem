using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
