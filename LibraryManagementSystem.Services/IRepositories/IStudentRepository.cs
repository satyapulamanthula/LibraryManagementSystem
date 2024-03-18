using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Repository.IRepositories
{
    public interface IStudentRepository
    {
        List<StudentEnrolmentModel> GetAllStudents();
        void CreateStudent(StudentEnrolmentModel student);
    }
}
