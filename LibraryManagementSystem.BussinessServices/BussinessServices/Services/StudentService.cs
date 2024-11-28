using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<StudentEnrolmentModel>> GetAllStudents()
        {
            return await _studentRepository.GetAllStudents();
        }
        public async Task CreateStudent(StudentEnrolmentModel student)
        {
            await _studentRepository.CreateStudent(student);
        }
    }
}
