using Moq;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Domain.ValueObjects;
using MusicSchool.SchoolManagement.Repositories;

namespace MusicSchool.SchoolManagement.Domain.Tests.Services;

public class EnrollmentServiceTests
{
    private readonly Mock<IRepository<Enrollment>> _enrollmentRepositoryMock;
    private readonly EnrollmentService _sut;

    public EnrollmentServiceTests()
    {
        _enrollmentRepositoryMock = new Mock<IRepository<Enrollment>>();
        _sut = new EnrollmentService(_enrollmentRepositoryMock.Object);
    }

    [Fact]
    public async Task EnrollAsync_WithValidInput_AddsEnrollment()
    {
        Guid studentId = Guid.NewGuid();
        Guid courseId = Guid.NewGuid();
        DateOnly startDate = new(2023, 1, 1);
        DateOnly endDate = new(2023, 12, 31);
        int lessonsPerMonth = 4;
        BrlAmount monthlyBill = 200;

        Enrollment enrollment = await _sut.EnrollAsync(
            studentId,
            courseId,
            startDate,
            endDate,
            lessonsPerMonth,
            monthlyBill
        );

        Assert.NotEqual(Guid.Empty, enrollment.Id);
        Assert.Equal(studentId, enrollment.StudentId);
        Assert.Equal(courseId, enrollment.CourseId);
        Assert.Equal(startDate, enrollment.StartDate);
        Assert.Equal(endDate, enrollment.EndDate);
        Assert.Equal(lessonsPerMonth, enrollment.LessonsPerMonth);
        Assert.Equal(monthlyBill, enrollment.MonthlyBill);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(enrollment), Times.Once);
    }

    [Fact]
    public async Task EnrollAsync_WithInvalidPeriod_ThrowsDomainException()
    {
        Guid studentId = Guid.NewGuid();
        Guid courseId = Guid.NewGuid();
        DateOnly startDate = new(2023, 12, 31);
        DateOnly endDate = new(2023, 1, 1);
        int lessonsPerMonth = 4;
        BrlAmount monthlyBill = 200;

        await Assert.ThrowsAsync<DomainException>(() => _sut.EnrollAsync(
            studentId,
            courseId,
            startDate,
            endDate,
            lessonsPerMonth,
            monthlyBill
        ));

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }

    [Fact]
    public async Task EnrollAsync_WithInvalidLessonsPerMonth_ThrowsDomainException()
    {
        Guid studentId = Guid.NewGuid();
        Guid courseId = Guid.NewGuid();
        DateOnly startDate = new(2023, 1, 1);
        DateOnly endDate = new(2023, 12, 31);
        int lessonsPerMonth = 0;
        BrlAmount monthlyBill = 200;

        await Assert.ThrowsAsync<DomainException>(() => _sut.EnrollAsync(
            studentId,
            courseId,
            startDate,
            endDate,
            lessonsPerMonth,
            monthlyBill
        ));

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }

    [Fact]
    public async Task EnrollAsync_WithInvalidMonthlyBill_ThrowsDomainException()
    {
        Guid studentId = Guid.NewGuid();
        Guid courseId = Guid.NewGuid();
        DateOnly startDate = new(2023, 1, 1);
        DateOnly endDate = new(2023, 12, 31);
        int lessonsPerMonth = 4;
        BrlAmount monthlyBill = -1;

        await Assert.ThrowsAsync<DomainException>(() => _sut.EnrollAsync(
            studentId,
            courseId,
            startDate,
            endDate,
            lessonsPerMonth,
            monthlyBill
        ));

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }
}
