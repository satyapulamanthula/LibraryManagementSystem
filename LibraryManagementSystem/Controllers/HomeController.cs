using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LibraryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookService BookService;
        private readonly ILogger<HomeController> Logger;

        public HomeController(IBookService bookService,ILogger<HomeController> logger)
        {
            BookService = bookService;
            Logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            //// Check if the user is authenticated and in the Admin role
            //if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            //{
            //    ViewData["Layout"] = "~/Views/Shared/AdminLayout.cshtml";
            //}
            //else
            //{
            //    ViewData["Layout"] = "~/Views/Shared/UserLayout.cshtml";
            //}

            return View();
        }

        public IActionResult Privacy()
        {
            var roleData = BookService.GetAllBooks();
            return View(roleData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}