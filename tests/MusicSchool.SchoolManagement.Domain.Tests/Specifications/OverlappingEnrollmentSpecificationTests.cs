using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class OverlappingEnrollmentSpecificationTests
{
    [Fact]
    public void IsSatisfiedBy_WithEqualDates_ReturnsTrue()
    {
        Enrollment enrollment = new()
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
        };

        OverlappingEnrollmentSpecification sut = new OverlappingEnrollmentSpecification(
            new DateOnly(2023, 1, 1),
            new DateOnly(2023, 12, 31)
        );

        Assert.True(sut.IsSatisfiedBy(enrollment));
    }

    [Fact]
    public void IsSatisfiedBy_WithLeftOverlap_ReturnsTrue()
    {
        Enrollment enrollment = new()
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
        };

        OverlappingEnrollmentSpecification sut = new OverlappingEnrollmentSpecification(
            new DateOnly(2022, 7, 1),
            new DateOnly(2023, 6, 30)
        );

        Assert.True(sut.IsSatisfiedBy(enrollment));
    }

    [Fact]
    public void IsSatisfiedBy_WithRightOverlap_ReturnsTrue()
    {
        Enrollment enrollment = new()
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
        };

        OverlappingEnrollmentSpecification sut = new OverlappingEnrollmentSpecification(
            new DateOnly(2023, 7, 1),
            new DateOnly(2024, 6, 30)
        );

        Assert.True(sut.IsSatisfiedBy(enrollment));
    }

    [Fact]
    public void IsSatisfiedBy_WithInnerOverlap_ReturnsTrue()
    {
        Enrollment enrollment = new()
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
        };

        OverlappingEnrollmentSpecification sut = new OverlappingEnrollmentSpecification(
            new DateOnly(2023, 6, 1),
            new DateOnly(2023, 7, 31)
        );

        Assert.True(sut.IsSatisfiedBy(enrollment));
    }

    [Fact]
    public void IsSatisfiedBy_WithOuterOverlap_ReturnsTrue()
    {
        Enrollment enrollment = new()
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
        };

        OverlappingEnrollmentSpecification sut = new OverlappingEnrollmentSpecification(
            new DateOnly(2022, 7, 1),
            new DateOnly(2024, 6, 30)
        );

        Assert.True(sut.IsSatisfiedBy(enrollment));
    }

    [Fact]
    public void IsSatisfiedBy_WithoutOverlap_ReturnsFalse()
    {
        Enrollment enrollment = new()
        {
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 12, 31),
        };

        OverlappingEnrollmentSpecification sut = new OverlappingEnrollmentSpecification(
            new DateOnly(2024, 1, 1),
            new DateOnly(2024, 12, 31)
        );

        Assert.False(sut.IsSatisfiedBy(enrollment));
    }
}
