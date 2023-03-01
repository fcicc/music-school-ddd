using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public class ActiveStudentSpecification : ISpecification<Student>
{
    public ActiveStudentSpecification(DateMonthOnly atMonth, IQueryable<Enrollment> enrollments)
    {
        AtMonth = atMonth;
        Enrollments = enrollments;
    }

    public DateMonthOnly AtMonth { get; }

    public IQueryable<Enrollment> Enrollments { get; }

    public Expression<Func<Student, bool>> AsPredicate()
    {
        return s => Enrollments.Any(
            e => e.StudentId == s.Id &&
            e.StartMonth <= AtMonth &&
            e.EndMonth >= AtMonth
        );
    }
}
