using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class StudentNameSpecificationTests
{
    [Theory]
    [InlineData("Luiz Melodia", true)]
    [InlineData("José da Silva", false)]
    public void IsSatisfiedBy_WithSpecifiedName_ReturnsExpectedResult(
        string name,
        bool expectedResult)
    {
        Student student = new()
        {
            Name = "Luiz Melodia"
        };

        StudentNameSpecification sut = new(name);

        Assert.Equal(expectedResult, sut.IsSatisfiedBy(student));
    }
}
