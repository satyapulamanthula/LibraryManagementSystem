using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.Repositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryManagementSystem.Repository.Test
{
    public class BookRepositoryTests
    {
        [Fact]
        public async Task CreateBook_ShouldAddBookToDbContext()
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
                var loggerMock = new Mock<ILogger<BookRepository>>();  // Mocking ILogger
                var cacheMock = new Mock<IDistributedCache>();  // Mocking IDistributedCache
                var repository = new BookRepository(dbContext, cacheMock.Object, loggerMock.Object);

                var book = new BookModel
                {
                    BookId = 1,
                    BookName = "New Book",
                    AuthorName = "New Author",
                    Price = 20.99m
                };

                // Act
                await repository.CreateBook(book);  // Make sure to await the asynchronous method

                // Assert
                var result = await dbContext.Books.FindAsync(book.BookId);  // Use FindAsync to ensure async operation
                Assert.NotNull(result);
                Assert.Equal(book.BookName, result.BookName);
                Assert.Equal(book.AuthorName, result.AuthorName);
                Assert.Equal(book.Price, result.Price);
            }
        }

    }
}

