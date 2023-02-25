using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Tests.Specifications;

public class StudentEnrolledOnCourseSpecificationTests
{
    [Fact]
    public void IsSatisfiedBy_WithMatchingStudentAndCourseIds_ReturnsTrue()
    {
        Enrollment enrollment = new()
        {
            StudentId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
        };

        StudentEnrolledOnCourseSpecification sut = new StudentEnrolledOnCourseSpecification(
            enrollment.StudentId,
            enrollment.CourseId
        );

        Assert.True(sut.IsSatisfiedBy(enrollment));
    }

    [Fact]
    public void IsSatisfiedBy_WithMatchingStudentIdOnly_ReturnsFalse()
    {
        Enrollment enrollment = new()
        {
            StudentId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
        };

        StudentEnrolledOnCourseSpecification sut = new StudentEnrolledOnCourseSpecification(
            enrollment.StudentId,
            Guid.NewGuid()
        );

        Assert.False(sut.IsSatisfiedBy(enrollment));
    }

    [Fact]
    public void IsSatisfiedBy_WithMatchingCourseIdOnly_ReturnsFalse()
    {
        Enrollment enrollment = new()
        {
            StudentId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
        };

        StudentEnrolledOnCourseSpecification sut = new StudentEnrolledOnCourseSpecification(
            Guid.NewGuid(),
            enrollment.CourseId
        );

        Assert.False(sut.IsSatisfiedBy(enrollment));
    }

    [Fact]
    public void IsSatisfiedBy_WithNothingMatching_ReturnsFalse()
    {
        Enrollment enrollment = new()
        {
            StudentId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
        };

        StudentEnrolledOnCourseSpecification sut = new StudentEnrolledOnCourseSpecification(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        Assert.False(sut.IsSatisfiedBy(enrollment));
    }
}
