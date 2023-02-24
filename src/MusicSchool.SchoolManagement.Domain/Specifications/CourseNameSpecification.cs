using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public class CourseNameSpecification : ISpecification<Course>
{
    public CourseNameSpecification(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public Expression<Func<Course, bool>> AsPredicate()
    {
        return c => c.Name == Name;
    }
}
