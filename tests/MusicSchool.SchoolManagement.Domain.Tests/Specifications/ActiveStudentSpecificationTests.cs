using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class ActiveStudentSpecificationTests
{
    [Theory]
    [MemberData(nameof(GenerateTestData))]
    public void IsSatisfiedBy_WithSpecifiedInfo_ReturnsExpectedResult(
        DateMonthOnly atMonth,
        bool expectedResult)
    {
        Student student = new()
        {
            Id = Guid.NewGuid(),
            Name = "Luiz Melodia",
        };

        Student otherStudent = new()
        {
            Id = Guid.NewGuid(),
            Name = "José da Silva",
        };

        Course course = new()
        {
            Id = Guid.NewGuid(),
            Name = "Técnica Vocal",
        };

        Enrollment[] enrollments = new Enrollment[]
        {
            new()
            {
                Id = Guid.NewGuid(),
                StudentId = student.Id,
                CourseId = course.Id,
                StartMonth = new DateMonthOnly(2023, 1),
                EndMonth = new DateMonthOnly(2023, 12),
                MonthlyBillingValue = 200,
            },
            new()
            {
                Id = Guid.NewGuid(),
                StudentId = otherStudent.Id,
                CourseId = course.Id,
                StartMonth = new DateMonthOnly(2024, 1),
                EndMonth = new DateMonthOnly(2024, 12),
            },
        };

        ActiveStudentSpecification sut = new(atMonth, enrollments.AsQueryable());

        Assert.Equal(expectedResult, sut.IsSatisfiedBy(student));
    }

    public static IEnumerable<object[]> GenerateTestData()
    {
        yield return new object[]
        {
            new DateMonthOnly(2023, 1),
            true,
        };

        yield return new object[]
        {
            new DateMonthOnly(2023, 6),
            true,
        };

        yield return new object[]
        {
            new DateMonthOnly(2023, 12),
            true,
        };

        yield return new object[]
        {
            new DateMonthOnly(2022, 12),
            false
        };

        yield return new object[]
        {
            new DateMonthOnly(2024, 1),
            false,
        };
    }
}
