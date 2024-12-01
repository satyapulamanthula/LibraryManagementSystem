using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.IServices
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentEnrolmentModel>> GetAllStudents();
        Task CreateStudent(StudentEnrolmentModel student);
    }
}
