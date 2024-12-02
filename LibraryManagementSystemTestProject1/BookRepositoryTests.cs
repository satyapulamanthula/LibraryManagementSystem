using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.Repositories;
using LibraryManagementSystem.SharedModels.Models;
using System.Linq;
using LibraryManagementSystem.Repository.IRepositories;

namespace LibraryManagementSystem.Tests.Repositories
{
    public class BookRepositoryTests
    {
        private readonly LibraryDbContext _dbContext;
        private readonly Mock<ILibraryManagementLogger> _mockLogger;
        private readonly BookRepository _bookRepository;

        public BookRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "LibraryTestDb")
                .Options;

            _dbContext = new LibraryDbContext(options); // Use real in-memory database
            _mockLogger = new Mock<ILibraryManagementLogger>();
            _bookRepository = new BookRepository(_dbContext, _mockLogger.Object);
        }

        [Fact]
        public void CreateBook_ValidBook_AddsBookAndLogsSuccess()
        {
            // Arrange
            var bookModel = new BookModel
            {
                BookId = 1,
                BookName = "Test Book",
                AuthorName = "Test Author",
                Publishing = "Test Publisher",
                Price = 100,
                Subject = "Test Subject"
            };

            // Act
            _bookRepository.CreateBook(bookModel);

            // Assert
            var addedBook = _dbContext.Books.FirstOrDefault(b => b.BookId == 1);
            Assert.NotNull(addedBook);
            Assert.Equal("Test Book", addedBook.BookName);
            _mockLogger.Verify(x => x.LogInformation(It.Is<string>(s => s.Contains("is added successfully"))), Times.Once);
        }

        [Fact]
        public void CreateBookCategory_ValidCategory_AddsCategoryAndLogsSuccess()
        {
            // Arrange
            var categoryModel = new BookCategories
            {
                Subject = "Test Category"
            };

            // Act
            _bookRepository.CreateBookCategory(categoryModel);

            // Assert
            var addedCategory = _dbContext.BookCategories.FirstOrDefault(c => c.Subject == "Test Category");
            Assert.NotNull(addedCategory);
            Assert.Equal("Test Category", addedCategory.Subject);
            _mockLogger.Verify(x => x.LogInformation(It.Is<string>(s => s.Contains("is added successfully"))), Times.Once);
        }

        [Fact]
        public void CreateBookCategory_DuplicateCategory_DoesNotAddCategoryAndLogsWarning()
        {
            // Arrange
            _dbContext.BookCategories.Add(new BooksCategory { Subject = "Duplicate Category" });
            _dbContext.SaveChanges();

            var categoryModel = new BookCategories
            {
                Subject = "Duplicate Category"
            };

            // Act
            _bookRepository.CreateBookCategory(categoryModel);

            // Assert
            Assert.Equal(2, _dbContext.BookCategories.Count()); // Ensure no new category was added
            _mockLogger.Verify(x => x.LogWarning(It.Is<string>(s => s.Contains("already exists"))), Times.Once);
        }
    }
}



// Ignore the below code.
//using LibraryManagementSystem.Data.Entities;
//using LibraryManagementSystem.Repository.Repositories;
//using LibraryManagementSystem.SharedModels.Models;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Xunit;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using LibraryManagementSystem.Repository.IRepositories;

//namespace LibraryManagementSystem.Tests.Repositories
//{
//    public class BookRepositoryTests
//    {
//        private readonly Mock<LibraryDbContext> _mockDbContext;
//        private readonly Mock<ILibraryManagementLogger> _mockLogger;
//        private readonly BookRepository _bookRepository;

//        public BookRepositoryTests()
//        {
//            _mockDbContext = new Mock<LibraryDbContext>();
//            _mockLogger = new Mock<ILibraryManagementLogger>();
//            _bookRepository = new BookRepository(_mockDbContext.Object, _mockLogger.Object);
//        }

//        [Fact]
//        public void CreateBook_ValidBook_AddsBookAndLogsSuccess()
//        {
//            // Arrange
//            var bookModel = new BookModel
//            {
//                BookId = 1,
//                BookName = "Test Book",
//                AuthorName = "Test Author",
//                Publishing = "Test Publisher",
//                Price = 100,
//                Subject = "Test Subject"
//            };

//            var bookDbSet = new Mock<DbSet<Book>>();
//            _mockDbContext.Setup(x => x.Books).Returns(bookDbSet.Object);

//            // Act
//            _bookRepository.CreateBook(bookModel);

//            // Assert
//            bookDbSet.Verify(x => x.Add(It.IsAny<Book>()), Times.Once); // Verify Add was called
//            _mockDbContext.Verify(x => x.SaveChanges(), Times.Once); // Verify SaveChanges was called
//            _mockLogger.Verify(x => x.LogInformation(It.Is<string>(s => s.Contains("is added successfully"))), Times.Once);
//        }

//        [Fact]
//        public void CreateBookCategory_ValidCategory_AddsCategoryAndLogsSuccess()
//        {
//            // Arrange
//            var categoryModel = new BookCategories
//            {
//                Subject = "Test Category"
//            };

//            var categoryDbSet = new Mock<DbSet<BooksCategory>>();
//            _mockDbContext.Setup(x => x.BookCategories).Returns(categoryDbSet.Object);

//            // Simulate no existing category
//            _mockDbContext.Setup(x => x.BookCategories.FirstOrDefault(It.IsAny<Func<BooksCategory, bool>>()))
//                .Returns((BooksCategory)null);

//            // Act
//            _bookRepository.CreateBookCategory(categoryModel);

//            // Assert
//            categoryDbSet.Verify(x => x.Add(It.Is<BooksCategory>(c => c.Subject == "Test Category")), Times.Once);
//            _mockDbContext.Verify(x => x.SaveChanges(), Times.Once);
//            _mockLogger.Verify(x => x.LogInformation(It.Is<string>(s => s.Contains("is added successfully"))), Times.Once);
//        }

//        [Fact]
//        public void CreateBookCategory_DuplicateCategory_DoesNotAddCategoryAndLogsWarning()
//        {
//            // Arrange
//            var categoryModel = new BookCategories
//            {
//                Subject = "Duplicate Category"
//            };

//            var categoryDbSet = new Mock<DbSet<BooksCategory>>();
//            _mockDbContext.Setup(x => x.BookCategories).Returns(categoryDbSet.Object);

//            // Simulate existing category
//            _mockDbContext.Setup(x => x.BookCategories.FirstOrDefault(It.IsAny<Func<BooksCategory, bool>>()))
//                .Returns(new BooksCategory { Subject = "Duplicate Category" });

//            // Act
//            _bookRepository.CreateBookCategory(categoryModel);

//            // Assert
//            categoryDbSet.Verify(x => x.Add(It.IsAny<BooksCategory>()), Times.Never); // Ensure Add is not called
//            _mockDbContext.Verify(x => x.SaveChanges(), Times.Never); // Ensure SaveChanges is not called
//            _mockLogger.Verify(x => x.LogWarning(It.Is<string>(s => s.Contains("already exists"))), Times.Once);
//        }

//    }
//}










////Ignor it
////using LibraryManagementSystem.Data.Entities;
////using LibraryManagementSystem.Repository.IRepositories;
////using LibraryManagementSystem.Repository.Repositories;
////using LibraryManagementSystem.SharedModels.Models;
////using Microsoft.EntityFrameworkCore;
////using Moq;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using Xunit;

////namespace LibraryManagementSystem.Repository.Test
////{
////    public class BookRepositoryTests
////    {
////        [Fact]
////        public void GetAllBooks_ShouldReturnListOfBooks()
////        {
////            // Arrange
////            var dbContextOptions = new DbContextOptionsBuilder<LibraryDbContext>()
////                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
////                .Options;

////            var dbContext = new LibraryDbContext(dbContextOptions); // Initialize your context with in-memory options

////            var loggerMock = new Mock<ILibraryManagementLogger>();
////            var repository = new BookRepository(dbContext, loggerMock.Object);

////            // Add some dummy data to the in-memory database
////            dbContext.Books.Add(new Book { BookId = 1, BookName = "Book1", AuthorName = "Author1", Publishing = "Publisher1", Subject = "Subject1" });
////            dbContext.Books.Add(new Book { BookId = 2, BookName = "Book2", AuthorName = "Author2", Publishing = "Publisher2", Subject = "Subject2" });
////            dbContext.SaveChanges();

////            // Act
////            var result = repository.GetAllBooks();

////            // Assert
////            Assert.NotNull(result);
////            Assert.IsType<List<Book>>(result);
////            Assert.Equal(2, result.Count); // Adjust the count based on the number of dummy books you added
////        }



////        [Fact]
////        public void CreateBook_ShouldAddBookToDbContext()
////        {
////            // Arrange
////            var options = new DbContextOptionsBuilder<LibraryDbContext>()
////                .UseInMemoryDatabase(databaseName: "TestDatabase")
////                .Options;

////            using (var dbContext = new LibraryDbContext(options))
////            {
////                // Ensure a fresh database for each test
////                dbContext.Database.EnsureDeleted();
////                dbContext.Database.EnsureCreated();
////            }

////            using (var dbContext = new LibraryDbContext(options))
////            {
////                // Act
////                var loggerMock = new Mock<ILibraryManagementLogger>();
////                var repository = new BookRepository(dbContext, loggerMock.Object);

////                var book = new BookModel
////                {
////                    BookId = 1,
////                    BookName = "New Book",
////                    AuthorName = "New Author",
////                    Publishing = "Some Publisher",
////                    Price = 20.99m,
////                    Subject = "Some Subject"
////                };

////                repository.CreateBook(book);

////                // Assert
////                //dbContext.Entry(book).State = EntityState.Detached; // Detach the entity to prevent tracking
////                var result = dbContext.Books.Find(book.BookId);

////                Assert.NotNull(result);
////                Assert.Equal(book.BookName, result.BookName);
////                Assert.Equal(book.AuthorName, result.AuthorName);
////                Assert.Equal(book.Publishing, result.Publishing);
////                Assert.Equal(book.Price, result.Price);
////                Assert.Equal(book.Subject, result.Subject);
////            }
////        }

////    }
////}
