using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//[Authorize(Roles = "Admin, Staff, Librarian, Student")]
public class BooksController : Controller
{
    private readonly IBookService BookService;

    public BooksController(IBookService bookService)
    {
        BookService = bookService;
    }

    public IActionResult Index()
    {
        // Your Books view logic here
        return View();
    }

    [Authorize(Roles = "Admin, Librarian")]
    public IActionResult AddNewBook()
    {
        return View();
    }

    // POST
    [HttpPost]
    public IActionResult AddNewBook(BookModel book)
    {
        //if (book != null)
        //{
        //    _bookService.CreateBook(book);

        //    // Set success message
        //    ViewBag.SuccessMessage = "Book added successfully.";
        //    return RedirectToAction("AddNewBook");
        //}
        //else
        //{
        //    return View(book);
        //}
        if (book != null)
        {
            BookService.CreateBook(book);

            // Set success message in TempData
            TempData["SuccessMessage"] = "Book added successfully.";
            return RedirectToAction("AddNewBook");
        }
        else
        {
            return View(book);
        }
    }

    public IActionResult AddNewCategory(/*BookCategories categoryName */string categoryName)
    {
        if (!string.IsNullOrEmpty(categoryName))
        {
            // Create a new instance of BookCategories and set its properties
            var newCategory = new BookCategories
            {
                Subject = categoryName
                // Set other properties of BookCategories here if needed
            };

            // Save the new category to the database
            BookService.CreateCategory(newCategory);
            // Set success message
            ViewBag.SuccessMessage = "BookCategorie added successfully.";
            // Return a JSON response indicating success
            return Json(new { success = true });
        }
        else
        {
            // Return a JSON response indicating failure
            return Json(new { success = false, errorMessage = "Category name is required." });
        }
    }

    public async Task<IActionResult> ViewBooks()
    {
        var books = await BookService.GetAllBooks();
        return View(books);
    }


    public async  Task<IActionResult> BookCategories()
    {
        var categories = await BookService.GetBookCategories();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            // If the request is AJAX (dropdown request), return JSON data
            return Json(categories);
        }
        else
        {
            // If it's a regular request, return the view with the model
            return View(categories);
        }
    }

    //public IActionResult GetAllSemestersData()
    //{
    //    var Semesters = _bookService.GetSemestersData();
    //    return Json(Semesters);
    //}

}
