using LibraryManagementSystem.Services.BussinessServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class CompleteBooksDetailsController : Controller
    {
        private readonly IIssuedBookService _issuedBookService;

        public CompleteBooksDetailsController(IIssuedBookService issuedBookService)
        {
            _issuedBookService = issuedBookService;
        }
        public IActionResult Index()
        {
            var viewIssuedBooks = _issuedBookService.GetAllIssuedBooks();
            return View(viewIssuedBooks);
        }
    }
}
