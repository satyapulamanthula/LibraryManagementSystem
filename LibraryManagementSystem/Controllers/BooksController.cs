 using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.SharedModels.Models;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data.Entities;

//[Authorize(Roles = "Admin, Staff, Librarian, Student")]
public class BooksController : Controller
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
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
            _bookService.CreateBook(book);

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
            _bookService.CreateCategory(newCategory);
            // Set success message
            ViewBag.SuccessMessage = "Student added successfully.";
            // Return a JSON response indicating success
            return Json(new { success = true });
        }
        else
        {
            // Return a JSON response indicating failure
            return Json(new { success = false, errorMessage = "Category name is required." });
        }
    }

    public IActionResult ViewBooks()
    {
        var books = _bookService.GetAllBooks();
        return View(books);
    }


    public IActionResult BookCategories()
    {
        var categories = _bookService.GetBookCategories();

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

}
