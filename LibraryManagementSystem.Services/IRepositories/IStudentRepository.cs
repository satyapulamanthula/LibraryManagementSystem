using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Repository.IRepositories
{
    public interface IStudentRepository
    {
        Task<List<StudentEnrolmentModel>> GetAllStudents();
        Task CreateStudent(StudentEnrolmentModel student);
    }
}
