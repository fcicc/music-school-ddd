using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Factories;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Tests.Services;

public class EnrollmentFactoryTests
{
    private readonly EnrollmentFactory _sut = new EnrollmentFactory();

    [Fact]
    public void CreateEnrollment_WithValidInput_ReturnsEnrollment()
    {
        Student student = new() { Id = Guid.NewGuid() };
        Course course = new() { Id = Guid.NewGuid() };
        DateOnly startDate = new DateOnly(2023, 1, 1);
        DateOnly endDate = new DateOnly(2023, 12, 31);
        int lessonsPerMonth = 4;
        BrlAmount monthlyBill = 200;

        Enrollment enrollment = _sut.CreateEnrollment(
            student,
            course,
            startDate,
            endDate,
            lessonsPerMonth,
            monthlyBill
        );

        Assert.NotEqual(Guid.Empty, enrollment.Id);
        Assert.Equal(student.Id, enrollment.StudentId);
        Assert.Equal(course.Id, enrollment.CourseId);
        Assert.Equal(startDate, enrollment.StartDate);
        Assert.Equal(endDate, enrollment.EndDate);
        Assert.Equal(lessonsPerMonth, enrollment.LessonsPerMonth);
        Assert.Equal(monthlyBill, enrollment.MonthlyBill);
    }

    [Fact]
    public void CreateEnrollment_WithInvalidPeriod_ThrowsDomainException()
    {
        Student student = new() { Id = Guid.NewGuid() };
        Course course = new() { Id = Guid.NewGuid() };
        DateOnly startDate = new DateOnly(2023, 12, 31);
        DateOnly endDate = new DateOnly(2023, 1, 1);
        int lessonsPerMonth = 4;
        BrlAmount monthlyBill = 200;

        Assert.Throws<DomainException>(() => _sut.CreateEnrollment(
            student,
            course,
            startDate,
            endDate,
            lessonsPerMonth,
            monthlyBill
        ));
    }

    [Fact]
    public void CreateEnrollment_WithInvalidLessonsPerMonth_ThrowsDomainException()
    {
        Student student = new() { Id = Guid.NewGuid() };
        Course course = new() { Id = Guid.NewGuid() };
        DateOnly startDate = new DateOnly(2023, 1, 1);
        DateOnly endDate = new DateOnly(2023, 12, 31);
        int lessonsPerMonth = 0;
        BrlAmount monthlyBill = 200;

        Assert.Throws<DomainException>(() => _sut.CreateEnrollment(
            student,
            course,
            startDate,
            endDate,
            lessonsPerMonth,
            monthlyBill
        ));
    }

    [Fact]
    public void CreateEnrollment_WithInvalidMonthlyBill_ThrowsDomainException()
    {
        Student student = new() { Id = Guid.NewGuid() };
        Course course = new() { Id = Guid.NewGuid() };
        DateOnly startDate = new DateOnly(2023, 1, 1);
        DateOnly endDate = new DateOnly(2023, 12, 31);
        int lessonsPerMonth = 4;
        BrlAmount monthlyBill = -1;

        Assert.Throws<DomainException>(() => _sut.CreateEnrollment(
            student,
            course,
            startDate,
            endDate,
            lessonsPerMonth,
            monthlyBill
        ));
    }
}
