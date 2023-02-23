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
    private readonly Mock<IRepository<Student>> _studentRepositoryMock;
    private readonly Mock<IRepository<Course>> _courseRepositoryMock;

    private readonly EnrollmentService _sut;

    public EnrollmentServiceTests()
    {
        _enrollmentRepositoryMock = new Mock<IRepository<Enrollment>>();
        _studentRepositoryMock = new Mock<IRepository<Student>>();
        _courseRepositoryMock = new Mock<IRepository<Course>>();

        _sut = new EnrollmentService(
            _enrollmentRepositoryMock.Object,
            _studentRepositoryMock.Object,
            _courseRepositoryMock.Object
        );
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

        _studentRepositoryMock.Setup(r => r.FindOneAsync(studentId))
            .ReturnsAsync(new Student
            {
                Id = studentId,
                Name = "Luiz Melodia",
            });

        _courseRepositoryMock.Setup(r => r.FindOneAsync(courseId))
            .ReturnsAsync(new Course
            {
                Id = courseId,
                Name = "Técnica Vocal",
            });

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
    public async Task EnrollAsync_WithNonExistingStudent_ThrowsDomainException()
    {
        Guid studentId = Guid.NewGuid();
        Guid courseId = Guid.NewGuid();
        DateOnly startDate = new(2023, 1, 1);
        DateOnly endDate = new(2023, 12, 31);
        int lessonsPerMonth = 4;
        BrlAmount monthlyBill = 200;

        _courseRepositoryMock.Setup(r => r.FindOneAsync(courseId))
            .ReturnsAsync(new Course
            {
                Id = courseId,
                Name = "Técnica Vocal",
            });

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(
                studentId,
                courseId,
                startDate,
                endDate,
                lessonsPerMonth,
                monthlyBill
            )
        );

        Assert.Equal("Student not found.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }

    [Fact]
    public async Task EnrollAsync_WithNonExistingCourse_ThrowsDomainException()
    {
        Guid studentId = Guid.NewGuid();
        Guid courseId = Guid.NewGuid();
        DateOnly startDate = new(2023, 1, 1);
        DateOnly endDate = new(2023, 12, 31);
        int lessonsPerMonth = 4;
        BrlAmount monthlyBill = 200;

        _studentRepositoryMock.Setup(r => r.FindOneAsync(studentId))
            .ReturnsAsync(new Student
            {
                Id = studentId,
                Name = "Luiz Melodia",
            });

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(
                studentId,
                courseId,
                startDate,
                endDate,
                lessonsPerMonth,
                monthlyBill
            )
        );

        Assert.Equal("Course not found.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
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

        _studentRepositoryMock.Setup(r => r.FindOneAsync(studentId))
            .ReturnsAsync(new Student
            {
                Id = studentId,
                Name = "Luiz Melodia",
            });

        _courseRepositoryMock.Setup(r => r.FindOneAsync(courseId))
            .ReturnsAsync(new Course
            {
                Id = courseId,
                Name = "Técnica Vocal",
            });

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(
                studentId,
                courseId,
                startDate,
                endDate,
                lessonsPerMonth,
                monthlyBill
            )
        );

        Assert.Equal("Start date cannot be after end date.", exception.Message);

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

        _studentRepositoryMock.Setup(r => r.FindOneAsync(studentId))
            .ReturnsAsync(new Student
            {
                Id = studentId,
                Name = "Luiz Melodia",
            });

        _courseRepositoryMock.Setup(r => r.FindOneAsync(courseId))
            .ReturnsAsync(new Course
            {
                Id = courseId,
                Name = "Técnica Vocal",
            });

        await Assert.ThrowsAsync<DomainException>(() => _sut.EnrollAsync(
            studentId,
            courseId,
            startDate,
            endDate,
            lessonsPerMonth,
            monthlyBill
        ));

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(
                studentId,
                courseId,
                startDate,
                endDate,
                lessonsPerMonth,
                monthlyBill
            )
        );

        Assert.Equal("Lessons per month should be greater than zero.", exception.Message);

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

        _studentRepositoryMock.Setup(r => r.FindOneAsync(studentId))
            .ReturnsAsync(new Student
            {
                Id = studentId,
                Name = "Luiz Melodia",
            });

        _courseRepositoryMock.Setup(r => r.FindOneAsync(courseId))
            .ReturnsAsync(new Course
            {
                Id = courseId,
                Name = "Técnica Vocal",
            });

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(
                studentId,
                courseId,
                startDate,
                endDate,
                lessonsPerMonth,
                monthlyBill
            )
        );

        Assert.Equal("Monthly bill cannot be less than zero.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }
}
