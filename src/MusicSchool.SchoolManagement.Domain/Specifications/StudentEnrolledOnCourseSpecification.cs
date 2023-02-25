using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public class StudentEnrolledOnCourseSpecification : ISpecification<Enrollment>
{
    public StudentEnrolledOnCourseSpecification(Guid studentId, Guid courseId)
    {
        StudentId = studentId;
        CourseId = courseId;
    }

    public Guid StudentId { get; }

    public Guid CourseId { get; }

    public Expression<Func<Enrollment, bool>> AsPredicate()
    {
        return e => e.StudentId == StudentId && e.CourseId == CourseId;
    }
}
