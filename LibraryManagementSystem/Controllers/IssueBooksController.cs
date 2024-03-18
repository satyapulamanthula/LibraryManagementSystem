using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.Services.BussinessServices.Services;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class IssueBooksController : Controller
    {
        private readonly IIssuedBookService _issuedBookService;

        public IssueBooksController(IIssuedBookService issuedBookService)
        {
            _issuedBookService = issuedBookService;
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
                bool isSuccess = _issuedBookService.IssuedBook(issued);

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
            var viewIssuedBooks = _issuedBookService.GetAllIssuedBooks();
            return View(viewIssuedBooks);
        }

        public IActionResult GetStudentDetails()
        {
            var studentDetails = _issuedBookService.GetAllStudentsDetails();
            return Json(studentDetails);
        }

        public IActionResult GetBookDetails()
        {
            var bookDetails = _issuedBookService.GetAllBooksDetails();
            return Json(bookDetails);
        }


    }
}
