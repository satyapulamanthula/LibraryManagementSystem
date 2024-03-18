using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Repository.Repositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class ReturnBookService : IReturnBookService
    {
        public readonly IReturnBookRepository _returnBookRepository;

        public ReturnBookService(IReturnBookRepository returnBookRepository)
        {
            _returnBookRepository = returnBookRepository;
        }

        public List<IssuedModel> GetAllIssuedBooksToStudent(int studentId, bool isReturn, bool isFinePaid)
        {
            return _returnBookRepository.GetAllIssuedBooks(studentId, isReturn, isFinePaid);
        }

        public List<IssuedModel> UpdateIssuedBook(List<int> issuedBookIds,int studentId, bool isReturn, bool isFinePaid)
        {
            _returnBookRepository.UpdatingBook(issuedBookIds);
            // Assuming UpdatingBook now returns the updated list, adjust accordingly
            return _returnBookRepository.GetAllIssuedBooks(studentId, isReturn, isFinePaid);
        }

        public List<IssuedModel> UpdateFine(List<int> issuedBookIds, int studentId, bool isFinePaid, bool isReturn)
        {
            _returnBookRepository.UpdatingFine(issuedBookIds);
            // Assuming UpdatingBook now returns the updated list, adjust accordingly
            return _returnBookRepository.GetAllIssuedBooks(studentId, isFinePaid, isReturn);
        }


    }
}
