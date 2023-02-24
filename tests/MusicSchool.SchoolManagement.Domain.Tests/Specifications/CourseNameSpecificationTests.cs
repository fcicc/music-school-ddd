using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class CourseNameSpecificationTests
{
    [Fact]
    public void AsQueryable_WithName_FiltersCourses()
    {
        const string filteredName = "Técnica Vocal";

        List<Course> courses = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = filteredName,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Guitarra",
            },
        };

        CourseNameSpecification sut = new(filteredName);

        List<Course> filteredList = courses.AsQueryable()
            .Where(sut.AsPredicate())
            .ToList();

        Assert.All(filteredList, c => Assert.Equal(filteredName, c.Name));
    }
}
