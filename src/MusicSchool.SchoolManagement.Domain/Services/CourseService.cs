using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;

namespace MusicSchool.SchoolManagement.Domain.Services;

public class CourseService : ICourseService
{
    private readonly IRepository<Course> _courseRepository;

    public CourseService(IRepository<Course> courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<Course> CreateAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new DomainException("Course name cannot be empty.");
        }

        Course course = new()
        {
            Id = Guid.NewGuid(),
            Name = name,
        };

        await _courseRepository.AddAsync(course);

        return course;
    }
}
