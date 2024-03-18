using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.Services;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementSystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Authorize]
    public class ReturnBooksController : Controller
    {
        private readonly IReturnBookService _returnBookService;

        public ReturnBooksController(IReturnBookService returnBookService)
        {
            _returnBookService = returnBookService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PayFine()
        {
            return View();
        }

        public IActionResult ViewIssuedBooks()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetStudentDetails(int studentId,bool isReturn, bool isFinePaid)
        {
            var studentDetails = _returnBookService.GetAllIssuedBooksToStudent(studentId, isReturn, isFinePaid);

            return Json(studentDetails);
        }

        [HttpPost]
        public IActionResult MarkBooksReturned(List<int> issuedBookIds, int studentId,bool isReturn, bool isFinePaid)
        {
            var updatingBook = _returnBookService.UpdateIssuedBook(issuedBookIds, studentId,isReturn, isFinePaid);

            return Json(new { success = true, message = "Books marked as returned successfully." });
        }

        [HttpPost]
        public IActionResult MarkFineReturn(List<int> issuedBookIds, int studentId, bool isReturn, bool isFinePaid)
        {
            var updatingBook = _returnBookService.UpdateFine(issuedBookIds, studentId, isReturn, isFinePaid);

            return Json(new { success = true, message = "Books marked as returned successfully." });
        }


    }
}