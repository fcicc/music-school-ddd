using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Services;

public interface ICourseService
{
    Task<Course> CreateAsync(CreateCourseRequest request);

    public class CreateCourseRequest
    {
        public string Name { get; init; } = "";
    }
}
