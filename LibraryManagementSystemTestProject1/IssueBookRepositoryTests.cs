using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LibraryManagementSystem.Tests.RepositoryTests
{
    public class IssuedRepositoryTests
    {
        [Fact]
        public void IssuedBook_ShouldAddIssuedBookToDbContext()
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
                var loggerMock = new Mock<ILibraryManagementLogger>();
                var repository = new IssueBookRepository(dbContext, loggerMock.Object);

                var issued = new Issued
                {
                    StudentId = 1,
                    BookId = 1,
                    IssueDate = new DateTime(2024, 3, 14),
                    IsReturn = false
                };

                var result = repository.IssuedBook(issued);

                // Assert
                Assert.True(result);

                // Check if the issued book is added to the database
                var issuedBook = dbContext.Issued.FirstOrDefault(i => i.StudentId == issued.StudentId && i.BookId == issued.BookId);

                Assert.NotNull(issuedBook);
                Assert.Equal(issued.StudentId, issuedBook.StudentId);
                Assert.Equal(issued.BookId, issuedBook.BookId);
                Assert.Equal(issued.IssueDate, issuedBook.IssueDate);
                Assert.False(issuedBook.IsReturn); // Ensure that IsReturn is set to false
            }
        }
    }
}
