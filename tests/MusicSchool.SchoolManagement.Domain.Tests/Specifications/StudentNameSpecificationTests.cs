using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class StudentNameSpecificationTests
{
    [Fact]
    public void IsSatisfiedBy_WithMatchingName_ReturnsTrue()
    {
        Student student = new()
        {
            Name = "Luiz Melodia"
        };

        StudentNameSpecification sut = new(student.Name);

        Assert.True(sut.IsSatisfiedBy(student));
    }

    [Fact]
    public void IsSatisfiedBy_WithNonMatchingName_ReturnsTrue()
    {
        Student student = new()
        {
            Name = "Luiz Melodia"
        };

        StudentNameSpecification sut = new("José da Silva");

        Assert.False(sut.IsSatisfiedBy(student));
    }
}
