﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Data.Entities
{
    public class Semesters
    {
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }

        // Navigation property
        public ICollection<Book> Books { get; set; }
    }
}
