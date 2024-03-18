using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Repository.IRepositories
{
    public interface IReturnBookRepository
    {
        List<IssuedModel> GetAllIssuedBooks(int studentId, bool isReturn, bool isFinePaid);

        void UpdatingBook(List<int> issuedBookIds);
        void UpdatingFine(List<int> issuedBookIds);
    }
}
