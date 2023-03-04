using MockQueryable.Moq;
using Moq;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;

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
        IEnrollmentService.EnrollRequest request = new()
        {
            StudentId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            StartMonth = new(2023, 1),
            EndMonth = new(2023, 12),
            MonthlyBillingValue = 200,
        };

        _studentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Student>
            {
                new()
                {
                    Id = request.StudentId,
                },
            }.BuildMock());

        _courseRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Course>
            {
                new()
                {
                    Id = request.CourseId,
                },
            }.BuildMock());

        _enrollmentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Enrollment>().BuildMock());

        Enrollment enrollment = await _sut.EnrollAsync(request);

        Assert.NotEqual(Guid.Empty, enrollment.Id);
        Assert.Equal(request.StudentId, enrollment.StudentId);
        Assert.Equal(request.CourseId, enrollment.CourseId);
        Assert.Equal(request.StartMonth, enrollment.StartMonth);
        Assert.Equal(request.EndMonth, enrollment.EndMonth);
        Assert.Equal(request.MonthlyBillingValue, enrollment.MonthlyBillingValue);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(enrollment), Times.Once);
    }

    [Fact]
    public async Task EnrollAsync_WithInvalidPeriod_ThrowsDomainException()
    {
        IEnrollmentService.EnrollRequest request = new()
        {
            StudentId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            StartMonth = new(2023, 12),
            EndMonth = new(2023, 1),
            MonthlyBillingValue = 200,
        };

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(request)
        );

        Assert.Equal("Start month cannot be after end month.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }

    [Fact]
    public async Task EnrollAsync_WithInvalidMonthlyBillingValue_ThrowsDomainException()
    {
        IEnrollmentService.EnrollRequest request = new()
        {
            StudentId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            StartMonth = new(2023, 1),
            EndMonth = new(2023, 12),
            MonthlyBillingValue = -1,
        };

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(request)
        );

        Assert.Equal("Monthly billing value cannot be less than zero.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }

    [Fact]
    public async Task EnrollAsync_WithNonExistingStudent_ThrowsDomainException()
    {
        IEnrollmentService.EnrollRequest request = new()
        {
            StudentId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            StartMonth = new(2023, 1),
            EndMonth = new(2023, 12),
            MonthlyBillingValue = 200,
        };

        _studentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Student>().BuildMock());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(request)
        );

        Assert.Equal("Student not found.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }

    [Fact]
    public async Task EnrollAsync_WithNonExistingCourse_ThrowsDomainException()
    {
        IEnrollmentService.EnrollRequest request = new()
        {
            StudentId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            StartMonth = new(2023, 1),
            EndMonth = new(2023, 12),
            MonthlyBillingValue = 200,
        };

        _studentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Student>
            {
                new()
                {
                    Id = request.StudentId,
                },
            }.BuildMock());

        _courseRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Course>().BuildMock());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(request)
        );

        Assert.Equal("Course not found.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }

    [Fact]
    public async Task EnrollAsync_WithOverlappingEnrollment_ThrowsDomainException()
    {
        IEnrollmentService.EnrollRequest request = new()
        {
            StudentId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            StartMonth = new(2023, 1),
            EndMonth = new(2023, 12),
            MonthlyBillingValue = 200,
        };

        _studentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Student>
            {
                new()
                {
                    Id = request.StudentId,
                },
            }.BuildMock());

        _courseRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Course>
            {
                new()
                {
                    Id = request.CourseId,
                },
            }.BuildMock());

        _enrollmentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Enrollment>
            {
                new()
                {
                    StudentId = request.StudentId,
                    CourseId = request.CourseId,
                    StartMonth = request.StartMonth,
                    EndMonth = request.EndMonth,
                    MonthlyBillingValue = request.MonthlyBillingValue
                }
            }.BuildMock());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.EnrollAsync(request)
        );

        Assert.Equal("There already exists an enrollment that overlaps the new one.", exception.Message);

        _enrollmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Enrollment>()), Times.Never);
    }
}
