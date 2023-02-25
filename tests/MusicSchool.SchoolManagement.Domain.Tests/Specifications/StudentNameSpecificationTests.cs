using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class StudentNameSpecificationTests
{
    [Theory]
    [InlineData("Luiz Melodia", "Luiz Melodia", true)]
    [InlineData("Luiz Melodia", "José da Silva", false)]
    public void IsSatisfiedBy_WithSpecifiedName_ReturnsExpectedResult(
        string studentName,
        string specifiedName,
        bool expectedResult)
    {
        Student student = new()
        {
            Name = studentName
        };

        StudentNameSpecification sut = new(specifiedName);

        Assert.Equal(expectedResult, sut.IsSatisfiedBy(student));
    }
}
