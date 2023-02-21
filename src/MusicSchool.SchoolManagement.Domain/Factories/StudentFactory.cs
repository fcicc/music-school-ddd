using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;

namespace MusicSchool.SchoolManagement.Domain.Factories;

public class StudentFactory : IStudentFactory
{
    public Student CreateStudent(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new DomainException("Student name cannot be empty.");
        }

        return new()
        {
            Id = Guid.NewGuid(),
            Name = name,
        };
    }
}
