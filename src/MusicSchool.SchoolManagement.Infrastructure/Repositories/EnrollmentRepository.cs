using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;

namespace MusicSchool.SchoolManagement.Infrastructure.Repositories;

public class EnrollmentRepository : Repository<Enrollment>
{
    public EnrollmentRepository(SchoolManagementContext context)
        : base(context)
    {
    }

    public override IQueryable<Enrollment> AsQueryable()
    {
        return base.AsQueryable()
            .OrderBy(e => e.StartMonth)
            .ThenBy(e => e.EndMonth);
    }
}
