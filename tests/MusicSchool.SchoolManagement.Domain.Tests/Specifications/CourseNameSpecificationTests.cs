using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class CourseNameSpecificationTests
{
    [Theory]
    [InlineData("Técnica Vocal", "Técnica Vocal", true)]
    [InlineData("Técnica Vocal", "Guitarra", false)]
    public void IsSatisfiedBy_WithSpecifiedName_ReturnsExpectedResult(
        string courseName,
        string specifiedName,
        bool expectedResult)
    {
        Course course = new()
        {
            Name = courseName
        };

        CourseNameSpecification sut = new(specifiedName);

        Assert.Equal(expectedResult, sut.IsSatisfiedBy(course));
    }
}
