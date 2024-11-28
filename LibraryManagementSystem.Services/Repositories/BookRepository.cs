using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;

namespace LibraryManagementSystem.Repository.Repositories
{

    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IDistributedCache _cache;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(LibraryDbContext dbContext, IDistributedCache cache, ILogger<BookRepository> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        //GetAllBooksWithCaching
        public async Task<List<Book>> GetAllBooks()
        {
            var cacheKey = "all_books";
            var cachedBooks = await _cache.GetStringAsync(cacheKey);
            if (cachedBooks != null)
            {
                return JsonConvert.DeserializeObject<List<Book>>(cachedBooks);
            }

            var books = await _dbContext.Books.ToListAsync();
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(books), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
            return books;
        }

        ////GetBooksPaged
        //public async Task<List<Book>> GetBooks(int pageNumber, int pageSize)
        //{
        //    return await _dbContext.Books
        //                           .Skip((pageNumber - 1) * pageSize)
        //                           .Take(pageSize)
        //                           .ToListAsync();
        //}

        public async Task<List<BooksCategory>> GetAllBookCatedgories()
        {
            return await _dbContext.BookCategories.ToListAsync();
        }

        public async Task CreateBook(BookModel book)
        {
            try
            {
                var bookEntity = new Book
                {
                    BookName = book.BookName,
                    AuthorName = book.AuthorName,
                    Price = book.Price
                };

                await _dbContext.Books.AddAsync(bookEntity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add book: {ex.Message}");
                throw;
            }
        }

        public async Task CreateBookCategory(BookCategories category)
        {
            try
            {
                if (category == null)
                {
                    _logger.LogWarning("Book Category is empty");
                    return;
                }

                // Checking that if a category with the same subject already exists or not
                var existingCategory = _dbContext.BookCategories.FirstOrDefault(c => c.Subject == category.Subject);

                if (existingCategory != null)
                {
                    _logger.LogWarning($"A category with the subject '{category.Subject}' already exists.");
                    return;
                }

                var newCategory = new BooksCategory
                {
                    Subject = category.Subject,
                };

                await _dbContext.BookCategories.AddAsync(newCategory); // Add the new category
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to add a category with Exception: {ex.StackTrace}");
                throw;
            }
        }
    }



    //public class BookRepository : IBookRepository
    //{
    //    private readonly LibraryDbContext DbContext;
    //    private readonly ILibraryManagementLogger Logger;

    //    public BookRepository(LibraryDbContext dbContext, ILibraryManagementLogger libraryManagementLogger)
    //    {
    //        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    //        Logger = libraryManagementLogger ?? throw new ArgumentNullException(nameof(libraryManagementLogger));
    //    }
    //    //public List<Book> GetAllBooks()
    //    //{
    //    //    return DbContext.Books.ToList();
    //    //}
    //    public async Task<List<Book>> GetAllBooks()
    //    {
    //        return await DbContext.Books.ToListAsync();
    //    }

    //    public async Task<List<BooksCategory>> GetAllBookCatedgories()
    //    {
    //        return await DbContext.BookCategories.ToListAsync();
    //    }

    //    public async Task CreateBook(BookModel book)
    //    {
    //        try
    //        {
    //            if (book == null)
    //            {
    //                Logger.LogWarning("Book Details are empty");
    //            }
    //            else
    //            {
    //                var bookEntity = new Book
    //                {
    //                    BookId = book.BookId,
    //                    BookName = book.BookName,
    //                    AuthorName = book.AuthorName,
    //                    Publishing = book.Publishing,
    //                    Price = book.Price,
    //                    Subject = book.Subject,
    //                };

    //               await DbContext.Books.AddAsync(bookEntity);
    //               await DbContext.SaveChangesAsync();

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.LogError($"Failed to add a book data with Exception: {ex.StackTrace}");
    //            throw;
    //        }
    //    }

    //    public async Task CreateBookCategory(BookCategories category)
    //    {
    //        try
    //        {
    //            if (category == null)
    //            {
    //                Logger.LogWarning("Book Category is empty");
    //                return;
    //            }

    //            // Checking that if a category with the same subject already exists or not
    //            var existingCategory = DbContext.BookCategories.FirstOrDefault(c => c.Subject == category.Subject);

    //            if (existingCategory != null)
    //            {
    //                Logger.LogWarning($"A category with the subject '{category.Subject}' already exists.");
    //                return;
    //            }

    //            var newCategory = new BooksCategory
    //            {
    //                Subject = category.Subject,
    //            };

    //            await DbContext.BookCategories.AddAsync(newCategory); // Add the new category
    //            await DbContext.SaveChangesAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.LogError($"Failed to add a category with Exception: {ex.StackTrace}");
    //            throw;
    //        }
    //    }
    //}
}
