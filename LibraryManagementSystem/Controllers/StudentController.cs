using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService StudentService;

        public StudentController(IStudentService studentRepository)
        {
            StudentService = studentRepository;
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
                    StudentService.CreateStudent(studentEnrolment);

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

        public async Task<IActionResult> ViewStudents()
        {
            var students = await StudentService.GetAllStudents();
            return View(students);
        }
    }
}
