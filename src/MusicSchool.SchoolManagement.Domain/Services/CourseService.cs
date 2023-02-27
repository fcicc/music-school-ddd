using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Services;

public class CourseService : ICourseService
{
    private readonly IRepository<Course> _courseRepository;

    public CourseService(IRepository<Course> courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<Course> CreateAsync(ICourseService.CreateRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new DomainException("Course name cannot be empty.");
        }

        List<Course> existingCourses = await _courseRepository.FindAsync(
            new CourseNameSpecification(request.Name)
        );
        if (existingCourses.Any())
        {
            throw new DomainException("Course with same name already exists.");
        }

        Course course = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        await _courseRepository.AddAsync(course);

        return course;
    }
}
