using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;

namespace MusicSchool.SchoolManagement.Infrastructure.Repositories;

public class StudentRepository : Repository<Student>
{
    public StudentRepository(SchoolManagementContext context)
        : base(context)
    {
    }

    public override IQueryable<Student> AsQueryable()
    {
        return base.AsQueryable().OrderBy(s => s.Name);
    }
}
