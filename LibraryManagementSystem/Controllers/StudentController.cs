using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.Services.BussinessServices.Services;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentRepository;

        public StudentController(IStudentService studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddStudent()
        {
            // Your logic for adding new books
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(StudentEnrolmentModel studentEnrolment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _studentRepository.CreateStudent(studentEnrolment);

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




        //// POST
        //[HttpPost]
        //public IActionResult AddStudent(StudentEnrolmentModel studentEnrolment)
        //{
        //    if (ModelState.IsValid /*&& studentEnrolment != null*/)
        //    {
        //            _studentRepository.CreateStudent(studentEnrolment);

        //            //// Set success message
        //            //ViewBag.SuccessMessage = "Student added successfully.";

        //            // Set success message in TempData
        //            TempData["SuccessMessage"] = "Book added successfully.";

        //            return RedirectToAction("AddStudent");

        //    }
        //    else
        //    {
        //        return View(studentEnrolment);
        //    }
        //}

        public IActionResult ViewStudents()
        {
            var students = _studentRepository.GetAllStudents();
            return View(students);
        }
    }
}
