using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Services;

public interface IStudentService
{
    Task<Student> CreateAsync(CreateStudentRequest request);

    public class CreateStudentRequest
    {
        public string Name { get; init; } = "";
    }
}
