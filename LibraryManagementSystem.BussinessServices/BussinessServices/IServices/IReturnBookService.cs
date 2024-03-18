using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.BussinessServices.IServices
{
    public interface IReturnBookService
    {
        List<IssuedModel> GetAllIssuedBooksToStudent(int studentId, bool isReturn, bool isFinePaid);

        List<IssuedModel> UpdateIssuedBook(List<int> issuedBookIds, int studentId, bool isReturn, bool isFinePaid);

        List<IssuedModel> UpdateFine(List<int> issuedBookIds, int studentId, bool isReturn, bool isFinePaid);

    }
}
