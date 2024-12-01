using LibraryManagementSystem.Services.BussinessServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    //[Authorize]
    [Authorize(Roles = "Admin")]
    public class CompleteBooksDetailsController : Controller
    {
        private readonly IIssuedBookService IssuedBookService;

        public CompleteBooksDetailsController(IIssuedBookService issuedBookService)
        {
            IssuedBookService = issuedBookService;
        }
        public IActionResult Index()
        {
            var viewIssuedBooks = IssuedBookService.GetAllIssuedBooks();
            return View(viewIssuedBooks);
        }
    }
}
