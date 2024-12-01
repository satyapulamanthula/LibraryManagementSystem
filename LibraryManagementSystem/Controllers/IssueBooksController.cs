using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class IssueBooksController : Controller
    {
        private readonly IIssuedBookService IssuedBookService;

        public IssueBooksController(IIssuedBookService issuedBookService)
        {
            IssuedBookService = issuedBookService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult Index(Issued issued)
        {
            if (issued.BookId != 0 && issued.StudentId != 0)
            {
                // Call the service method to issue the book
                bool isSuccess = IssuedBookService.IssuedBook(issued);

                // Check if the book issuance was successful
                if (!isSuccess)
                {
                    // Set a warning message in TempData
                    TempData["WarningMessage"] = "Student has already issued three books.";
                    return RedirectToAction("Index");
                }

                // If successful, set success message in TempData
                TempData["SuccessMessage"] = "Book Issued successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(issued);
            }
        }

        public IActionResult ViewIssuedBooks()
        {
            var viewIssuedBooks = IssuedBookService.GetAllIssuedBooks();
            return View(viewIssuedBooks);
        }

        public IActionResult GetStudentDetails()
        {
            var studentDetails = IssuedBookService.GetAllStudentsDetails();
            return Json(studentDetails);
        }

        public IActionResult GetBookDetails()
        {
            var bookDetails = IssuedBookService.GetAllBooksDetails();
            return Json(bookDetails);
        }


    }
}
