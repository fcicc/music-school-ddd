using Microsoft.EntityFrameworkCore;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;
using MusicSchool.SchoolManagement.Repositories;

namespace MusicSchool.SchoolManagement.Infrastructure.Repositories;

public class StudentRepository : IRepository<Student>
{
    private readonly SchoolManagementContext _context;

    public StudentRepository(SchoolManagementContext context)
    {
        _context = context;
    }

    public Task AddAsync(Student student)
    {
        _context.Students.Add(student);
        return _context.SaveChangesAsync();
    }

    public Task<Student> FindOneAsync(Guid id)
    {
        return _context.Students
            .Where(s => s.Id == id)
            .FirstAsync();
    }
}
