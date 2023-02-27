using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Services;

public interface ICourseService
{
    Task<Course> CreateAsync(CreateRequest request);

    public class CreateRequest
    {
        public string Name { get; init; } = "";
    }
}
