using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.BussinessServices.IServices
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks();
        IEnumerable<BooksCategory> GetBookCategories();
        //IEnumerable<Semesters> GetSemestersData();
        void CreateBook(BookModel book);
        void CreateCategory(BookCategories bookCategories);
    }
}
