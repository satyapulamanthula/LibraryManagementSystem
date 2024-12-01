using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Repository.IRepositories
{
    public interface IReturnBookRepository
    {
        Task <List<IssuedModel>> GetAllIssuedBooks(int studentId, bool isReturn, bool isFinePaid);

        Task UpdatingBook(List<int> issuedBookIds);
        Task UpdatingFine(List<int> issuedBookIds);
    }
}
