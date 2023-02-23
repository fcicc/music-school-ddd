using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Repositories;

namespace MusicSchool.SchoolManagement.Domain.Services;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _studentRepository;

    public StudentService(IRepository<Student> studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Student> CreateAsync(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new DomainException("Student name cannot be empty.");
        }

        Student student = new()
        {
            Id = Guid.NewGuid(),
            Name = name,
        };

        await _studentRepository.AddAsync(student);

        return student;
    }
}
