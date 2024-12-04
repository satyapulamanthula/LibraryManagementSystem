using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Repository.Repositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryManagementSystem.Tests.RepositoryTests
{

    public class StudentRepositoryTests
    {
        [Fact]
        public void GetAllStudents_ShouldReturnListOfStudents()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "TempDatabase")
                .Options;

            var dbContext = new LibraryDbContext(dbContextOptions); // Initialize your context with in-memory options

            var loggerMock = new Mock<ILogger<StudentRepository>>();
            var repository = new StudentRepository(dbContext, loggerMock.Object);


            // Add some dummy data to the in-memory database
            dbContext.StudentEnrolments.Add(new StudentEnrolment
            {
                StudentId = 1,
                StudentName = "John Doe",
                Department = "Computer Science",
                Semester = "1",
                DateOfBirth = new DateTime(2000, 1, 1),
                StudentContact = "1234567890",
                StudentEmail = "john@example.com",
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 6, 30),
                IsActive = true
            });

            dbContext.StudentEnrolments.Add(new StudentEnrolment
            {
                StudentId = 2,
                StudentName = "John Doe2",
                Department = "Computer Science2",
                Semester = "1",
                DateOfBirth = new DateTime(2000, 12, 12),
                StudentContact = "1234567899",
                StudentEmail = "john2@example.com",
                StartDate = new DateTime(2024, 1, 11),
                EndDate = new DateTime(2024, 6, 30),
                IsActive = true
            });
            dbContext.SaveChanges();


            // Act
            var result = repository.GetAllStudents(); // Call the method under test

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<StudentEnrolmentModel>>(result.Result);
            Assert.Equal(2, result.Result.Count); // Adjust the count based on the number of dummy books you added
        }

        [Fact]
        public async Task CreateStudent_ShouldAddStudentToDbContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new LibraryDbContext(options))
            {
                // Ensure a fresh database for each test
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            using (var dbContext = new LibraryDbContext(options))
            {
                // Act
                var loggerMock = new Mock<ILogger<StudentRepository>>();
                var repository = new StudentRepository(dbContext, loggerMock.Object);

                var student = new StudentEnrolmentModel
                {
                    StudentName = "New Student123",
                    StudentEmail = "NewEmail@gmail.com",
                    StartDate = new DateTime(2024, 3, 14),
                    EndDate = new DateTime(2024, 10, 15),
                    StudentContact = "9876543210",
                    Department = "IT",
                    Semester = "11"
                };

                await repository.CreateStudent(student);  // Ensure this is awaited as it is async

                // Assert
                var result = await dbContext.StudentEnrolments
                    .FirstOrDefaultAsync(s => s.StudentEmail == student.StudentEmail);

                Assert.NotNull(result);  // Ensure the student is found
                Assert.Equal(student.StudentName, result.StudentName);
                Assert.Equal(student.StudentEmail, result.StudentEmail);
            }
        }
    }
}
