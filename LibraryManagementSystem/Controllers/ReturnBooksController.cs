using LibraryManagementSystem.Services.BussinessServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Authorize]
    public class ReturnBooksController : Controller
    {
        private readonly IReturnBookService ReturnBookService;

        public ReturnBooksController(IReturnBookService returnBookService)
        {
            ReturnBookService = returnBookService;
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
            var studentDetails = ReturnBookService.GetAllIssuedBooksToStudent(studentId, isReturn, isFinePaid);

            return Json(studentDetails);
        }

        [HttpPost]
        public IActionResult MarkBooksReturned(List<int> issuedBookIds, int studentId,bool isReturn, bool isFinePaid)
        {
            var updatingBook = ReturnBookService.UpdateIssuedBook(issuedBookIds, studentId,isReturn, isFinePaid);

            return Json(new { success = true, message = "Books marked as returned successfully." });
        }

        [HttpPost]
        public IActionResult MarkFineReturn(List<int> issuedBookIds, int studentId, bool isReturn, bool isFinePaid)
        {
            var updatingBook = ReturnBookService.UpdateFine(issuedBookIds, studentId, isReturn, isFinePaid);

            return Json(new { success = true, message = "Books marked as returned successfully." });
        }


    }
}