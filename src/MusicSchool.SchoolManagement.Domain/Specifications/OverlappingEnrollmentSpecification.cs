using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public class OverlappingEnrollmentSpecification : ISpecification<Enrollment>
{
    private readonly Guid _studentId;
    private readonly Guid _courseId;
    private readonly DateMonthOnly _startMonth;
    private readonly DateMonthOnly _endMonth;

    public OverlappingEnrollmentSpecification(
        Guid studentId,
        Guid courseId,
        DateMonthOnly startMonth,
        DateMonthOnly endMonth)
    {
        _studentId = studentId;
        _courseId = courseId;
        _startMonth = startMonth;
        _endMonth = endMonth;
    }

    public Expression<Func<Enrollment, bool>> AsPredicate()
    {
        return e =>
            e.StudentId == _studentId &&
            e.CourseId == _courseId &&
            (
                (e.StartMonth >= _startMonth && e.StartMonth <= _endMonth) ||
                (e.EndMonth >= _startMonth && e.EndMonth <= _endMonth) ||
                (_startMonth >= e.StartMonth && _startMonth <= e.EndMonth) ||
                (_endMonth >= e.StartMonth && _endMonth <= e.EndMonth)
            );
    }
}
