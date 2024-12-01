using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class ReturnBookService : IReturnBookService
    {
        public readonly IReturnBookRepository ReturnBookRepository;

        public ReturnBookService(IReturnBookRepository returnBookRepository)
        {
            ReturnBookRepository = returnBookRepository;
        }

        public async Task<List<IssuedModel>> GetAllIssuedBooksToStudent(int studentId, bool isReturn, bool isFinePaid)
        {
            return await ReturnBookRepository.GetAllIssuedBooks(studentId, isReturn, isFinePaid);
        }

        public async Task<List<IssuedModel>> UpdateIssuedBook(List<int> issuedBookIds,int studentId, bool isReturn, bool isFinePaid)
        {
            await ReturnBookRepository.UpdatingBook(issuedBookIds);
            // Assuming UpdatingBook now returns the updated list, adjust accordingly
            return await ReturnBookRepository.GetAllIssuedBooks(studentId, isReturn, isFinePaid);
        }

        public async Task<List<IssuedModel>> UpdateFine(List<int> issuedBookIds, int studentId, bool isFinePaid, bool isReturn)
        {
            await ReturnBookRepository.UpdatingFine(issuedBookIds);
            // Assuming UpdatingBook now returns the updated list, adjust accordingly
            return await ReturnBookRepository.GetAllIssuedBooks(studentId, isFinePaid, isReturn);
        }


    }
}
