using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.Repositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Xunit;

namespace LibraryManagementSystem.Repository.Test
{
    public class BookRepositoryTests
    {
        [Fact]
        public void GetAllBooks_ShouldReturnListOfBooks()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var dbContext = new LibraryDbContext(dbContextOptions); // Initialize your context with in-memory options

            var loggerMock = new Mock<ILogger<BookRepository>>();  // Mocking ILogger
            var cacheMock = new Mock<IDistributedCache>();  // Mocking IDistributedCache
            var repository = new BookRepository(dbContext, cacheMock.Object, loggerMock.Object);

            // Add some dummy data to the in-memory database
            dbContext.Books.Add(new Book { BookId = 1, BookName = "Book1", AuthorName = "Author1", Publishing = "Publisher1", Subject = "Subject1" });
            dbContext.Books.Add(new Book { BookId = 2, BookName = "Book2", AuthorName = "Author2", Publishing = "Publisher2", Subject = "Subject2" });
            dbContext.SaveChanges();  // Ensure data is saved to the in-memory database

            // Act
            var result = repository.GetAllBooks();  // Call the method to retrieve all books

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Book>>(result.Result);
            Assert.Equal(2, result.Result.Count); // Adjust the count based on the number of dummy books you added
        }

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


//using LibraryManagementSystem.Data.Entities;
//using LibraryManagementSystem.Repository.IRepositories;
//using LibraryManagementSystem.Repository.Repositories;
//using LibraryManagementSystem.SharedModels.Models;
//using Microsoft.EntityFrameworkCore;
//using Moq;

//namespace LibraryManagementSystem.Repository.Test
//{
//    public class BookRepositoryTests
//    {
//        [Fact]
//        public void GetAllBooks_ShouldReturnListOfBooks()
//        {
//            // Arrange
//            var dbContextOptions = new DbContextOptionsBuilder<LibraryDbContext>()
//                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
//                .Options;

//            var dbContext = new LibraryDbContext(dbContextOptions); // Initialize your context with in-memory options

//            var loggerMock = new Mock<ILibraryManagementLogger>();
//            var repository = new BookRepository(dbContext, loggerMock.Object);

//            // Add some dummy data to the in-memory database
//            dbContext.Books.Add(new Book { BookId = 1, BookName = "Book1", AuthorName = "Author1", Publishing = "Publisher1", Subject = "Subject1" });
//            dbContext.Books.Add(new Book { BookId = 2, BookName = "Book2", AuthorName = "Author2", Publishing = "Publisher2", Subject = "Subject2" });
//            dbContext.SaveChanges();

//            // Act
//            var result = repository.GetAllBooks();

//            // Assert
//            Assert.NotNull(result);
//            Assert.IsType<List<Book>>(result);
//            Assert.Equal(2, result.Count); // Adjust the count based on the number of dummy books you added
//        }



//        [Fact]
//        public void CreateBook_ShouldAddBookToDbContext()
//        {
//            // Arrange
//            var options = new DbContextOptionsBuilder<LibraryDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;

//            using (var dbContext = new LibraryDbContext(options))
//            {
//                // Ensure a fresh database for each test
//                dbContext.Database.EnsureDeleted();
//                dbContext.Database.EnsureCreated();
//            }

//            using (var dbContext = new LibraryDbContext(options))
//            {
//                // Act
//                var loggerMock = new Mock<ILibraryManagementLogger>();
//                var repository = new BookRepository(dbContext, loggerMock.Object);

//                var book = new BookModel
//                {
//                    BookId = 1,
//                    BookName = "New Book",
//                    AuthorName = "New Author",
//                    Publishing = "Some Publisher",
//                    Price = 20.99m,
//                    Subject = "Some Subject"
//                };

//                repository.CreateBook(book);

//                // Assert
//                //dbContext.Entry(book).State = EntityState.Detached; // Detach the entity to prevent tracking
//                var result = dbContext.Books.Find(book.BookId);

//                Assert.NotNull(result);
//                Assert.Equal(book.BookName, result.BookName);
//                Assert.Equal(book.AuthorName, result.AuthorName);
//                Assert.Equal(book.Publishing, result.Publishing);
//                Assert.Equal(book.Price, result.Price);
//                Assert.Equal(book.Subject, result.Subject);
//            }
//        }

//    }
//}
