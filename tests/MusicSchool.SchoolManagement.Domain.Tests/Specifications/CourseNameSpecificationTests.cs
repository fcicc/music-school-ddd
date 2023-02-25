using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class CourseNameSpecificationTests
{
    [Fact]
    public void IsSatisfiedBy_WithMatchingName_ReturnsTrue()
    {
        Course course = new()
        {
            Name = "Técnica Vocal"
        };

        CourseNameSpecification sut = new(course.Name);

        Assert.True(sut.IsSatisfiedBy(course));
    }

    [Fact]
    public void IsSatisfiedBy_WithNonMatchingName_ReturnsTrue()
    {
        Course course = new()
        {
            Name = "Técnica Vocal"
        };

        CourseNameSpecification sut = new("Guitarra");

        Assert.False(sut.IsSatisfiedBy(course));
    }
}
