using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.Services.BussinessServices.Services;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentRepository)
        {
            _studentService = studentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(StudentEnrolmentModel studentEnrolment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _studentService.CreateStudent(studentEnrolment);

                    TempData["SuccessMessage"] = "Student added successfully.";

                    return RedirectToAction("AddStudent");
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Student with the same ID or email already exists")
                    {
                        ModelState.AddModelError("", "Student with the same ID or email already exists.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to add student. Please try again later.");
                    }

                    return View(studentEnrolment);
                }
            }
            else
            {
                return View(studentEnrolment);
            }
        }

        public IActionResult ViewStudents()
        {
            var students = _studentService.GetAllStudents();
            return View(students);
        }
    }
}
