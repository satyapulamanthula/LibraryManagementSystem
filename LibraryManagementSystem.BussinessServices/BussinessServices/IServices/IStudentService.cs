using LibraryManagementSystem.SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.BussinessServices.IServices
{
    public interface IStudentService
    {
        IEnumerable<StudentEnrolmentModel> GetAllStudents();
        void CreateStudent(StudentEnrolmentModel student);
    }
}
