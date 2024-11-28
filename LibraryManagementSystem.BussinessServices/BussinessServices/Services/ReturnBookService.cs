using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class ReturnBookService : IReturnBookService
    {
        public readonly IReturnBookRepository _returnBookRepository;

        public ReturnBookService(IReturnBookRepository returnBookRepository)
        {
            _returnBookRepository = returnBookRepository;
        }

        public async Task<List<IssuedModel>> GetAllIssuedBooksToStudent(int studentId, bool isReturn, bool isFinePaid)
        {
            return await _returnBookRepository.GetAllIssuedBooks(studentId, isReturn, isFinePaid);
        }

        public async Task<List<IssuedModel>> UpdateIssuedBook(List<int> issuedBookIds,int studentId, bool isReturn, bool isFinePaid)
        {
            await _returnBookRepository.UpdatingBook(issuedBookIds);
            // Assuming UpdatingBook now returns the updated list, adjust accordingly
            return await _returnBookRepository.GetAllIssuedBooks(studentId, isReturn, isFinePaid);
        }

        public async Task<List<IssuedModel>> UpdateFine(List<int> issuedBookIds, int studentId, bool isFinePaid, bool isReturn)
        {
            await _returnBookRepository.UpdatingFine(issuedBookIds);
            // Assuming UpdatingBook now returns the updated list, adjust accordingly
            return await _returnBookRepository.GetAllIssuedBooks(studentId, isFinePaid, isReturn);
        }


    }
}
