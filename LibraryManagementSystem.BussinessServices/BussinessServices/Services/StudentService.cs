using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository StudentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            StudentRepository = studentRepository;
        }

        public async Task<IEnumerable<StudentEnrolmentModel>> GetAllStudents()
        {
            return await StudentRepository.GetAllStudents();
        }
        public async Task CreateStudent(StudentEnrolmentModel student)
        {
            await StudentRepository.CreateStudent(student);
        }
    }
}
