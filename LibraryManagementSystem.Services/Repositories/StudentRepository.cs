using LibraryManagementSystem.SharedModels.Models;
using LibraryManagementSystem.Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Data.Entities;

namespace LibraryManagementSystem.Repository.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly LibraryDbContext _dbContext;

        private ILibraryManagementLogger _logger;

        public StudentRepository(LibraryDbContext dbContext, ILibraryManagementLogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public List<StudentEnrolmentModel> GetAllStudents()
        {
            List<StudentEnrolmentModel> students = _dbContext.StudentEnrolments
                .Select(se => new StudentEnrolmentModel
                {
                    StudentId = se.StudentId,
                    StudentName = se.StudentName,
                    Department = se.Department,
                    Semester = se.Semester,
                    DateOfBirth = se.DateOfBirth,
                    StudentContact = se.StudentContact,
                    StudentEmail = se.StudentEmail,
                    StartDate = se.StartDate,
                    EndDate = se.EndDate,
                    IsActive = se.IsActive
                }).ToList();

            return students;
        }

        public void CreateStudent(StudentEnrolmentModel student)
        {
            try
            {
                if (student == null)
                {
                    _logger.LogWarning("Student Details are empty");
                    return;
                }

                // Checking that student with the same ID or email already exists or not
                var existingStudent = _dbContext.StudentEnrolments.FirstOrDefault(s => s.StudentId == student.StudentId || s.StudentEmail == student.StudentEmail);
                if (existingStudent != null)
                {
                    _logger.LogWarning($"Student with ID {student.StudentId} or email {student.StudentEmail} already exists");
                    throw new Exception("Student with the same ID or email already exists");
                }

                var studentEntity = new StudentEnrolment
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                    Department = student.Department,
                    Semester = student.Semester,
                    DateOfBirth = student.DateOfBirth,
                    StudentContact = student.StudentContact,
                    StudentEmail = student.StudentEmail,
                    StartDate = student.StartDate,
                    EndDate = student.EndDate
                };

                _dbContext.StudentEnrolments.Add(studentEntity);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add a Student data with Exception: {ex.StackTrace}");
                throw;
            }
        }
    }
}
