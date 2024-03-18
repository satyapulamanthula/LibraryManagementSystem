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
                    // Copy properties from StudentEnrolment to StudentEnrolment1
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
                    return; // Exit method if student is null
                }

                // Check if student with the same ID or email already exists
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

                // After SaveChanges, the student entity will be updated with the generated StudentId
                int studentId = studentEntity.StudentId;

                _logger.LogInformation($"Student (ID: {studentId}) is added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add a Student data with Exception: {ex.StackTrace}");
                throw;
            }
        }


        //public void CreateStudent(StudentEnrolmentModel student)
        //{
        //    try
        //    {
        //        if (student == null)
        //        {
        //            _logger.LogWarning("Student Details are empty");
        //        }
        //        else
        //        {
        //            var studentEntity = new StudentEnrolment
        //            {
        //                StudentId = student.StudentId,
        //                StudentName = student.StudentName,
        //                Department = student.Department,
        //                Semester = student.Semester,
        //                DateOfBirth = student.DateOfBirth,
        //                StudentContact = student.StudentContact,
        //                StudentEmail = student.StudentEmail,
        //                StartDate = student.StartDate,
        //                EndDate = student.EndDate
        //            };

        //            var studentdetails = studentEntity;
        //            //_dbContext.StudentEnrolments.Add(studentEntity);
        //            //_dbContext.SaveChanges();

        //            // After SaveChanges, the student entity will be updated with the generated StudentId
        //            int studentId = studentEntity.StudentId;

        //            _logger.LogInformation($"Student (ID: {studentId}) is added successfully");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Failed to add a Student data with Exception: {ex.StackTrace}");
        //        throw;
        //    }
        //}



    }
}
