using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Services;

public interface IStudentService
{
    Task<Student> CreateAsync(CreateRequest request);

    public class CreateRequest
    {
        public string Name { get; init; } = "";
    }
}
