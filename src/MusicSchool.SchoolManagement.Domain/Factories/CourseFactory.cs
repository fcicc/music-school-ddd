using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Exceptions;

namespace MusicSchool.SchoolManagement.Factories;

public class CourseFactory : ICourseFactory
{
    public Course CreateCourse(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new DomainException("Course name cannot be empty.");
        }

        return new()
        {
            Id = Guid.NewGuid(),
            Name = name,
        };
    }
}
