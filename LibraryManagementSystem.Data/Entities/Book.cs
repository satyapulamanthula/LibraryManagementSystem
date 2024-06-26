﻿using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Entities
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string Publishing { get; set; }
        public decimal Price { get; set; }
        public string Subject { get; set; }

        //// Foreign key properties
        //public int CategoryId { get; set; }
        //public int SemesterId { get; set; }

        //// Navigation properties
        //public BooksCategory Category { get; set; }
        //public Semesters Semester { get; set; }
    }
}
