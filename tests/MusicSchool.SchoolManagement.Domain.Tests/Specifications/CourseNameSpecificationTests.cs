using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class CourseNameSpecificationTests
{
    [Theory]
    [InlineData("Técnica Vocal", true)]
    [InlineData("Guitarra", false)]
    public void IsSatisfiedBy_WithSpecifiedName_ReturnsExpectedResult(
        string name,
        bool expectedResult)
    {
        Course course = new()
        {
            Name = "Técnica Vocal"
        };

        CourseNameSpecification sut = new(name);

        Assert.Equal(expectedResult, sut.IsSatisfiedBy(course));
    }
}
