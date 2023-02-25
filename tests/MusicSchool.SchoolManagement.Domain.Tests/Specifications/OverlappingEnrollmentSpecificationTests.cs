using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class OverlappingEnrollmentSpecificationTests
{
    [Theory]
    [MemberData(nameof(GenerateTestData))]
    public void IsSatisfiedBy_WithSpecifiedInfo_ReturnsExpectedResult(
        Guid studentId,
        Guid courseId,
        DateMonthOnly startMonth,
        DateMonthOnly endMonth,
        bool expectedResult)
    {
        Enrollment enrollment = new()
        {
            StudentId = Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            CourseId = Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            StartMonth = new DateMonthOnly(2023, 1),
            EndMonth = new DateMonthOnly(2023, 12),
        };

        OverlappingEnrollmentSpecification sut = new(
            studentId,
            courseId,
            startMonth,
            endMonth
        );

        Assert.Equal(expectedResult, sut.IsSatisfiedBy(enrollment));
    }

    public static IEnumerable<object[]> GenerateTestData()
    {
        // Equal months
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateMonthOnly(2023, 1),
            new DateMonthOnly(2023, 12),
            true,
        };

        // Left overlapping months
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateMonthOnly(2022, 7),
            new DateMonthOnly(2023, 6),
            true,
        };

        // Right overlapping months
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateMonthOnly(2023, 7),
            new DateMonthOnly(2024, 6),
            true,
        };

        // Inner overlapping months
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateMonthOnly(2023, 6),
            new DateMonthOnly(2023, 7),
            true,
        };

        // Outer overlapping months
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateMonthOnly(2022, 7),
            new DateMonthOnly(2024, 6),
            true,
        };

        // Non-overlapping months
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateMonthOnly(2024, 1),
            new DateMonthOnly(2024, 12),
            false,
        };

        // Non-matching student
        yield return new object[]
        {
            Guid.Parse("7ef130c0-e35a-4951-9924-a7cd0d370b6f"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateMonthOnly(2023, 1),
            new DateMonthOnly(2023, 12),
            false,
        };

        // Non-matching course
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("a37d57c3-a756-448c-a219-a0dc9c1d904c"),
            new DateMonthOnly(2023, 1),
            new DateMonthOnly(2023, 12),
            false,
        };
    }
}
