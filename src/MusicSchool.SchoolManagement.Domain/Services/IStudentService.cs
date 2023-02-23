using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Services;

public interface IStudentService
{
    Task<Student> CreateAsync(string name);
}
