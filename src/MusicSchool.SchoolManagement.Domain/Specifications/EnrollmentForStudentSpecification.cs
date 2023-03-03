using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public class EnrollmentForStudentSpecification : ISpecification<Enrollment>
{
    public EnrollmentForStudentSpecification(Guid studentId)
    {
        StudentId = studentId;
    }

    public Guid StudentId { get; }

    public Expression<Func<Enrollment, bool>> AsPredicate()
    {
        return e => e.StudentId == StudentId;
    }
}
