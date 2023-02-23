using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Services;

public interface ICourseService
{
    Task<Course> CreateAsync(string name);
}
