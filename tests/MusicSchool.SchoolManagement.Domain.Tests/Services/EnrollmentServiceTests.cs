using Moq;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Domain.Specifications;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Tests.Services;

public class EnrollmentServiceTests
{
    private readonly Mock<IRepository<Enrollment>> _enrollmentRepositoryMock;
    private readonly Mock<IRepository<Student>> _studentRepositoryMock;
    private readonly Mock<IRepository<Course>> _courseRepositoryMock;

    private readonly EnrollmentService _sut;

    public EnrollmentServiceTests()
    {
        _enrollmentRepositoryMock = new();
        _studentRepositoryMock = new();
        _courseRepositoryMock = new();

        _sut = new(
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
        BrlAmount monthlyBillingValue = 200;

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

        _enrollmentRepositoryMock
            .Setup(r => r.FindAsync(
                It.Is<OverlappingEnrollmentSpecification>(
                    s =>
                        s.StudentId == studentId &&
                        s.CourseId == courseId &&
                        s.StartDate == startDate &&
                        s.EndDate == endDate
                )
            ))
            .ReturnsAsync(new List<Enrollment>());

        Enrollment enrollment = await _sut.EnrollAsync(
            studentId,
            courseId,
            startDate,
            endDate,
            monthlyBillingValue
        );

        Assert.NotEqual(Guid.Empty, enrollment.Id);
        Assert.Equal(studentId, enrollment.StudentId);
        Assert.Equal(courseId, enrollment.CourseId);
        Assert.Equal(startDate, enrollment.StartDate);
        Assert.Equal(endDate, enrollment.EndDate);
        Assert.Equal(monthlyBillingValue, enrollment.MonthlyBillingValue);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(enrollment), Times.Once);
    }

    [Fact]
    public async Task EnrollAsync_WithNonExistingStudent_ThrowsDomainException()
    {
        Guid studentId = Guid.NewGuid();
        Guid courseId = Guid.NewGuid();
        DateOnly startDate = new(2023, 1, 1);
        DateOnly endDate = new(2023, 12, 31);
        BrlAmount monthlyBillingValue = 200;

        _courseRepositoryMock.Setup(r => r.FindOneAsync(courseId))
            .ReturnsAsync(new Course
            {
                Id = courseId,
                Name = "Técnica Vocal",
            });

        _enrollmentRepositoryMock
            .Setup(r => r.FindAsync(
                It.Is<OverlappingEnrollmentSpecification>(
                    s =>
                        s.StudentId == studentId &&
                        s.CourseId == courseId &&
                        s.StartDate == startDate &&
                        s.EndDate == endDate
                )
            ))
            .ReturnsAsync(new List<Enrollment>());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(
                studentId,
                courseId,
                startDate,
                endDate,
                monthlyBillingValue
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
        BrlAmount monthlyBillingValue = 200;

        _studentRepositoryMock.Setup(r => r.FindOneAsync(studentId))
            .ReturnsAsync(new Student
            {
                Id = studentId,
                Name = "Luiz Melodia",
            });

        _enrollmentRepositoryMock
            .Setup(r => r.FindAsync(
                It.Is<OverlappingEnrollmentSpecification>(
                    s =>
                        s.StudentId == studentId &&
                        s.CourseId == courseId &&
                        s.StartDate == startDate &&
                        s.EndDate == endDate
                )
            ))
            .ReturnsAsync(new List<Enrollment>());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(
                studentId,
                courseId,
                startDate,
                endDate,
                monthlyBillingValue
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
        BrlAmount monthlyBillingValue = 200;

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

        _enrollmentRepositoryMock
            .Setup(r => r.FindAsync(
                It.Is<OverlappingEnrollmentSpecification>(
                    s =>
                        s.StudentId == studentId &&
                        s.CourseId == courseId &&
                        s.StartDate == startDate &&
                        s.EndDate == endDate
                )
            ))
            .ReturnsAsync(new List<Enrollment>());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(
                studentId,
                courseId,
                startDate,
                endDate,
                monthlyBillingValue
            )
        );

        Assert.Equal("Start date cannot be after end date.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }

    [Fact]
    public async Task EnrollAsync_WithInvalidMonthlyBillingValue_ThrowsDomainException()
    {
        Guid studentId = Guid.NewGuid();
        Guid courseId = Guid.NewGuid();
        DateOnly startDate = new(2023, 1, 1);
        DateOnly endDate = new(2023, 12, 31);
        BrlAmount monthlyBillingValue = -1;

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

        _enrollmentRepositoryMock
            .Setup(r => r.FindAsync(
                It.Is<OverlappingEnrollmentSpecification>(
                    s =>
                        s.StudentId == studentId &&
                        s.CourseId == courseId &&
                        s.StartDate == startDate &&
                        s.EndDate == endDate
                )
            ))
            .ReturnsAsync(new List<Enrollment>());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(
                studentId,
                courseId,
                startDate,
                endDate,
                monthlyBillingValue
            )
        );

        Assert.Equal("Monthly billing value cannot be less than zero.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }

    [Fact]
    public async Task EnrollAsync_WithOverlappingEnrollment_ThrowsDomainException()
    {
        Guid studentId = Guid.NewGuid();
        Guid courseId = Guid.NewGuid();
        DateOnly startDate = new(2023, 1, 1);
        DateOnly endDate = new(2023, 12, 31);
        BrlAmount monthlyBillingValue = 200;

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

        _enrollmentRepositoryMock
            .Setup(r => r.FindAsync(
                It.Is<OverlappingEnrollmentSpecification>(
                    s =>
                        s.StudentId == studentId &&
                        s.CourseId == courseId &&
                        s.StartDate == startDate &&
                        s.EndDate == endDate
                )
            ))
            .ReturnsAsync(new List<Enrollment>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    StudentId = studentId,
                    CourseId = courseId,
                    StartDate = startDate,
                    EndDate = endDate,
                    MonthlyBillingValue = monthlyBillingValue
                }
            });

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(
                studentId,
                courseId,
                startDate,
                endDate,
                monthlyBillingValue
            )
        );

        Assert.Equal("There already exists an enrollment that overlaps the new one.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }
}
