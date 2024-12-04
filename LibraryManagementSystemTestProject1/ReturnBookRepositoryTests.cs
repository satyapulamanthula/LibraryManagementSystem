using Castle.Core.Logging;
using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryManagementSystem.Tests.RepositoryTests
{
    public class ReturnBookRepositoryTests
    {
        [Fact]
        public void UpdatingBook_ShouldUpdateIssuedBooks()
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

                // Add some test data
                var issuedBooks = new List<Issued>
                {
                    new Issued { IssueId = 1, IsReturn = false, IssueDate = DateTime.Now.AddDays(-10) },
                    new Issued { IssueId = 2, IsReturn = false, IssueDate = DateTime.Now.AddDays(-20) },
                    new Issued { IssueId = 3, IsReturn = false, IssueDate = DateTime.Now.AddDays(-30) }
                };
                dbContext.AddRange(issuedBooks);
                dbContext.SaveChanges();

                var loggerMock = new Mock<ILogger<ReturnBookRepository>>();
                var repository = new ReturnBookRepository(dbContext, loggerMock.Object);

                var issuedBookIds = new List<int> { 1, 2, 3 };

                // Act
                repository.UpdatingBook(issuedBookIds);

                // Assert
                foreach (var issuedBookId in issuedBookIds)
                {
                    var updatedIssuedBook = dbContext.Issued.Find(issuedBookId);
                    Assert.NotNull(updatedIssuedBook);
                    Assert.True(updatedIssuedBook.IsReturn);
                    Assert.NotNull(updatedIssuedBook.ReturnedDate);
                    // Add more assertions as needed
                }
            }
        }

        [Fact]
        public void UpdatingFine_ShouldUpdateFineForIssuedBooks()
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

                // Add some test data
                var issuedBooks = new List<Issued>
                {
                    new Issued { IssueId = 1, IsFinePaid = false },
                    new Issued { IssueId = 2, IsFinePaid = false },
                    new Issued { IssueId = 3, IsFinePaid = false }
                };
                dbContext.AddRange(issuedBooks);
                dbContext.SaveChanges();

                var loggerMock = new Mock<ILogger<ReturnBookRepository>>();
                var repository = new ReturnBookRepository(dbContext, loggerMock.Object);

                var issuedBookIds = new List<int> { 1, 2, 3 };

                // Act
                repository.UpdatingFine(issuedBookIds);

                // Assert
                foreach (var issuedBookId in issuedBookIds)
                {
                    var updatedIssuedBook = dbContext.Issued.Find(issuedBookId);
                    Assert.NotNull(updatedIssuedBook);
                    Assert.True(updatedIssuedBook.IsFinePaid);
                }
            }
        }
    }
}