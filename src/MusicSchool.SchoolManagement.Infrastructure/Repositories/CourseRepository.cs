using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;

namespace MusicSchool.SchoolManagement.Infrastructure.Repositories;

public class CourseRepository : Repository<Course>
{
    public CourseRepository(SchoolManagementContext context)
        : base(context)
    {
    }

    public override IQueryable<Course> AsQueryable()
    {
        return base.AsQueryable().OrderBy(c => c.Name);
    }
}
