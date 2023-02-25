using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public class OverlappingEnrollmentSpecification : ISpecification<Enrollment>
{
    public OverlappingEnrollmentSpecification(DateOnly startDate, DateOnly endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public DateOnly StartDate { get; }

    public DateOnly EndDate { get; }

    public Expression<Func<Enrollment, bool>> AsPredicate()
    {
        return e =>
            (e.StartDate >= StartDate && e.StartDate <= EndDate) ||
            (e.EndDate >= StartDate && e.EndDate <= EndDate) ||
            (StartDate >= e.StartDate && StartDate <= e.EndDate) ||
            (EndDate >= e.StartDate && EndDate <= e.EndDate);
    }
}
