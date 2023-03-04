using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public class ActiveStudentSpecification : ISpecification<Student>
{
    private readonly DateMonthOnly _atMonth;
    public readonly IQueryable<Enrollment> _enrollments;

    public ActiveStudentSpecification(DateMonthOnly atMonth, IQueryable<Enrollment> enrollments)
    {
        _atMonth = atMonth;
        _enrollments = enrollments;
    }

    public Expression<Func<Student, bool>> AsPredicate()
    {
        return s => _enrollments.Any(
            e => e.StudentId == s.Id &&
            e.StartMonth <= _atMonth &&
            e.EndMonth >= _atMonth
        );
    }
}
