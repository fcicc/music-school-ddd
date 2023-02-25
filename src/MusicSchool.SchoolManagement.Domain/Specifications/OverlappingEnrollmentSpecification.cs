using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public class OverlappingEnrollmentSpecification : ISpecification<Enrollment>
{
    public OverlappingEnrollmentSpecification(
        Guid studentId,
        Guid courseId,
        DateOnly startDate,
        DateOnly endDate)
    {
        StudentId = studentId;
        CourseId = courseId;
        StartDate = startDate;
        EndDate = endDate;
    }

    public Guid StudentId { get; }

    public Guid CourseId { get; }

    public DateOnly StartDate { get; }

    public DateOnly EndDate { get; }

    public Expression<Func<Enrollment, bool>> AsPredicate()
    {
        return e =>
            e.StudentId == StudentId &&
            e.CourseId == CourseId &&
            (
                (e.StartDate >= StartDate && e.StartDate <= EndDate) ||
                (e.EndDate >= StartDate && e.EndDate <= EndDate) ||
                (StartDate >= e.StartDate && StartDate <= e.EndDate) ||
                (EndDate >= e.StartDate && EndDate <= e.EndDate)
            );
    }
}
