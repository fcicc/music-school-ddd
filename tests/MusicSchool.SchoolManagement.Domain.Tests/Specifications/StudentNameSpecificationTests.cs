using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class StudentNameSpecificationTests
{
    [Fact]
    public void AsQueryable_WithName_FiltersStudents()
    {
        const string filteredName = "Luiz Melodia";

        List<Student> students = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = filteredName,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "José da Silva",
            },
        };

        StudentNameSpecification sut = new StudentNameSpecification(filteredName);

        List<Student> filteredList = students.AsQueryable()
            .Where(sut.AsPredicate())
            .ToList();

        Assert.All(filteredList, s => Assert.Equal(filteredName, s.Name));
    }
}
