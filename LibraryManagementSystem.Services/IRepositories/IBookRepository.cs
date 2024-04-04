﻿using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Repository.IRepositories
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
        List<BooksCategory> GetAllBookCatedgories();
        //List<Semesters> GetAllSemesters();
        void CreateBook(BookModel book);
        void CreateBookCategory(BookCategories bookCategories);
    }
}
