using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.IServices
{
    public interface IReturnBookService
    {
        Task<List<IssuedModel>> GetAllIssuedBooksToStudent(int studentId, bool isReturn, bool isFinePaid);

        Task<List<IssuedModel>> UpdateIssuedBook(List<int> issuedBookIds, int studentId, bool isReturn, bool isFinePaid);

        Task<List<IssuedModel>> UpdateFine(List<int> issuedBookIds, int studentId, bool isReturn, bool isFinePaid);

    }
}
