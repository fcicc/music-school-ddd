using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class EnrollmentForStudentSpecificationTests
{
    [Theory]
    [MemberData(nameof(GenerateTestData))]
    public void IsSatisfiedBy_WithSpecifiedStudentId_ReturnsExpectedResult(
        Guid studentId,
        bool expectedResult)
    {
        Enrollment enrollment = new()
        {
            StudentId = Guid.Parse("ab5ba687-0329-4de2-bbe8-1bc08fb258b9"),
        };

        EnrollmentForStudentSpecification sut = new(studentId);

        Assert.Equal(expectedResult, sut.IsSatisfiedBy(enrollment));
    }

    public static IEnumerable<object[]> GenerateTestData()
    {
        yield return new object[]
        {
            Guid.Parse("ab5ba687-0329-4de2-bbe8-1bc08fb258b9"),
            true
        };

        yield return new object[]
        {
            Guid.Parse("87b2359f-a3eb-438b-8c00-0435825488d7"),
            false
        };
    }
}
