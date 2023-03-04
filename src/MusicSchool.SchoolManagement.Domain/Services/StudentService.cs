using Microsoft.EntityFrameworkCore;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;

namespace MusicSchool.SchoolManagement.Domain.Services;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _studentRepository;

    public StudentService(IRepository<Student> studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<Student> CreateAsync(IStudentService.CreateStudentRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new DomainException("Student name cannot be empty.");
        }

        bool hasStudentWithName = await _studentRepository
           .AsQueryable()
           .AnyAsync(s => s.Name == request.Name);

        if (hasStudentWithName)
        {
            throw new DomainException("Student with same name already exists.");
        }

        Student student = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        await _studentRepository.AddAsync(student);

        return student;
    }
}
