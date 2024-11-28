﻿using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<StudentEnrolmentModel>> GetAllStudents()
        {
            var students = await _dbContext.StudentEnrolments
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
                    _logger.LogWarning("Student Details are empty");
                    return;
                }

                // Checking that student with the same ID or email already exists or not
                bool studentExists = await _dbContext.StudentEnrolments.AnyAsync(s => s.StudentId == student.StudentId || s.StudentEmail == student.StudentEmail);
                if (studentExists != null)
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

                await _dbContext.StudentEnrolments.AddAsync(studentEntity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add a Student data with Exception: {ex.StackTrace}");
                throw;
            }
        }
    }
}
