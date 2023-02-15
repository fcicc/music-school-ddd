using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Repositories;

namespace MusicSchool.SchoolManagement.Infrastructure.Repositories;

public class StudentRepository : IRepository<Student>
{
    public Task CreateAsync(Student entity)
    {
        throw new NotImplementedException();
    }

    public Task<Student> FindOneAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
