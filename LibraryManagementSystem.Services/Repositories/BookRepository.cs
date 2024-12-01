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
        private readonly LibraryDbContext DbContext;
        private readonly IDistributedCache Cache;
        private readonly ILogger<BookRepository> Logger;

        public BookRepository(LibraryDbContext dbContext, IDistributedCache cache, ILogger<BookRepository> logger)
        {
            DbContext = dbContext;
            Cache = cache;
            Logger = logger;
        }

        //GetAllBooksWithCaching
        public async Task<List<Book>> GetAllBooks()
        {
            var cacheKey = "all_books";
            var cachedBooks = await Cache.GetStringAsync(cacheKey);
            if (cachedBooks != null)
            {
                return JsonConvert.DeserializeObject<List<Book>>(cachedBooks);
            }

            var books = await DbContext.Books.ToListAsync();
            await Cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(books), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
            return books;
        }

        ////GetBooksPaged
        //public async Task<List<Book>> GetBooks(int pageNumber, int pageSize)
        //{
        //    return await DbContext.Books
        //                           .Skip((pageNumber - 1) * pageSize)
        //                           .Take(pageSize)
        //                           .ToListAsync();
        //}

        public async Task<List<BooksCategory>> GetAllBookCatedgories()
        {
            return await DbContext.BookCategories.ToListAsync();
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

                await DbContext.Books.AddAsync(bookEntity);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to add book: {ex.Message}");
                throw;
            }
        }

        public async Task CreateBookCategory(BookCategories category)
        {
            try
            {
                if (category == null)
                {
                    Logger.LogWarning("Book Category is empty");
                    return;
                }

                // Checking that if a category with the same subject already exists or not
                var existingCategory = DbContext.BookCategories.FirstOrDefault(c => c.Subject == category.Subject);

                if (existingCategory != null)
                {
                    Logger.LogWarning($"A category with the subject '{category.Subject}' already exists.");
                    return;
                }

                var newCategory = new BooksCategory
                {
                    Subject = category.Subject,
                };

                await DbContext.BookCategories.AddAsync(newCategory); // Add the new category
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to add a category with Exception: {ex.StackTrace}");
                throw;
            }
        }
    }
}
