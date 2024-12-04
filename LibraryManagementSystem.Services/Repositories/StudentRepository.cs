using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagementSystem.Repository.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ILogger<StudentRepository> Logger;
        private readonly LibraryDbContext DbContext;

        public StudentRepository(LibraryDbContext dbContext, ILogger<StudentRepository> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }

        public async Task<List<StudentEnrolmentModel>> GetAllStudents()
        {
            var students = await DbContext.StudentEnrolments
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
                }).ToListAsync();

            return students;
        }

        public async Task CreateStudent(StudentEnrolmentModel student)
        {
            try
            {
                if (student == null)
                {
                    Logger.LogWarning("Student Details are empty");
                    return;
                }

                // Checking that student with the same ID or email already exists or not
                bool studentExists = await DbContext.StudentEnrolments.AnyAsync(s => s.StudentId == student.StudentId || s.StudentEmail == student.StudentEmail);
                if (studentExists)
                {
                    Logger.LogWarning($"Student with ID {student.StudentId} or email {student.StudentEmail} already exists");
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

                await DbContext.StudentEnrolments.AddAsync(studentEntity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to add a Student data with Exception: {ex.StackTrace}");
                throw;
            }
        }
    }
}
