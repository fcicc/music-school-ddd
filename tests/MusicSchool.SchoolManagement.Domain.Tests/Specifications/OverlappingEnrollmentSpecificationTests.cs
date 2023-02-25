using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class OverlappingEnrollmentSpecificationTests
{
    [Theory]
    [MemberData(nameof(GenerateTestData))]
    public void IsSatisfiedBy_WithSpecifiedInfo_ReturnsExpectedResult(
        Guid studentId,
        Guid courseId,
        DateOnly startDate,
        DateOnly endDate,
        bool expectedResult)
    {
        Enrollment enrollment = new()
        {
            StudentId = Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            CourseId = Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
        };

        OverlappingEnrollmentSpecification sut = new(
            studentId,
            courseId,
            startDate,
            endDate
        );

        Assert.Equal(expectedResult, sut.IsSatisfiedBy(enrollment));
    }

    public static IEnumerable<object[]> GenerateTestData()
    {
        // Equal dates
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateOnly(2023, 1, 1),
            new DateOnly(2023, 12, 31),
            true,
        };

        // Left overlapping dates
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateOnly(2022, 7, 1),
            new DateOnly(2023, 6, 30),
            true,
        };

        // Right overlapping dates
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateOnly(2023, 7, 1),
            new DateOnly(2024, 6, 30),
            true,
        };

        // Inner overlapping dates
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateOnly(2023, 6, 1),
            new DateOnly(2023, 7, 31),
            true,
        };

        // Outer overlapping dates
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateOnly(2022, 7, 1),
            new DateOnly(2024, 6, 30),
            true,
        };

        // Non-overlapping dates
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateOnly(2024, 1, 1),
            new DateOnly(2024, 12, 31),
            false,
        };

        // Non-matching student
        yield return new object[]
        {
            Guid.Parse("7ef130c0-e35a-4951-9924-a7cd0d370b6f"),
            Guid.Parse("56f735da-73f8-4ee2-a8c5-bdd9323c33af"),
            new DateOnly(2023, 1, 1),
            new DateOnly(2023, 12, 31),
            false,
        };

        // Non-matching course
        yield return new object[]
        {
            Guid.Parse("87370ea2-9559-4521-8d51-3416fe5ff48a"),
            Guid.Parse("a37d57c3-a756-448c-a219-a0dc9c1d904c"),
            new DateOnly(2023, 1, 1),
            new DateOnly(2023, 12, 31),
            false,
        };
    }
}
