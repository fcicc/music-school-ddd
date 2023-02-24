using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public class StudentNameSpecification : ISpecification<Student>
{
    public StudentNameSpecification(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public Expression<Func<Student, bool>> AsPredicate()
    {
        return s => s.Name == Name;
    }
}
