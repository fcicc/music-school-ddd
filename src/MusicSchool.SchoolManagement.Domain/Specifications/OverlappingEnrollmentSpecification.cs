using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public class OverlappingEnrollmentSpecification : ISpecification<Enrollment>
{
    public OverlappingEnrollmentSpecification(
        Guid studentId,
        Guid courseId,
        DateMonthOnly startMonth,
        DateMonthOnly endMonth)
    {
        StudentId = studentId;
        CourseId = courseId;
        StartMonth = startMonth;
        EndMonth = endMonth;
    }

    public Guid StudentId { get; }

    public Guid CourseId { get; }

    public DateMonthOnly StartMonth { get; }

    public DateMonthOnly EndMonth { get; }

    public Expression<Func<Enrollment, bool>> AsPredicate()
    {
        return e =>
            e.StudentId == StudentId &&
            e.CourseId == CourseId &&
            (
                (e.StartMonth >= StartMonth && e.StartMonth <= EndMonth) ||
                (e.EndMonth >= StartMonth && e.EndMonth <= EndMonth) ||
                (StartMonth >= e.StartMonth && StartMonth <= e.EndMonth) ||
                (EndMonth >= e.StartMonth && EndMonth <= e.EndMonth)
            );
    }
}
