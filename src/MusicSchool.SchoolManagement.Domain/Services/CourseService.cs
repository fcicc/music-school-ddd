using Microsoft.EntityFrameworkCore;
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

    public async Task<Course> CreateAsync(ICourseService.CreateCourseRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new DomainException("Course name cannot be empty.");
        }

        bool hasCourseWithName = await _courseRepository
            .AsQueryable()
            .AnyAsync(c => c.Name == request.Name);

        if (hasCourseWithName)
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
