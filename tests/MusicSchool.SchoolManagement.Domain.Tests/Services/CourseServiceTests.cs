using MockQueryable.Moq;
using Moq;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;

namespace MusicSchool.SchoolManagement.Domain.Tests.Services;

public class CourseServiceTests
{
    private readonly Mock<IRepository<Course>> _courseRepositoryMock;

    private readonly CourseService _sut;

    public CourseServiceTests()
    {
        _courseRepositoryMock = new();

        _sut = new(_courseRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidName_ReturnsNewCourse()
    {
        ICourseService.CreateCourseRequest request = new()
        {
            Name = "Técnica Vocal",
        };

        _courseRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Course>().BuildMock());

        Course course = await _sut.CreateAsync(request);

        Assert.NotEqual(Guid.Empty, course.Id);
        Assert.Equal(request.Name, course.Name);

        _courseRepositoryMock.Verify(r => r.AddAsync(course), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithExistingName_ThrowsDomainException()
    {
        ICourseService.CreateCourseRequest request = new()
        {
            Name = "Técnica Vocal",
        };

        _courseRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Course>
            {
                new()
                {
                    Name = request.Name,
                },
            }.BuildMock());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("Course with same name already exists.", exception.Message);

        _courseRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Course>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyName_ThrowsDomainException()
    {
        ICourseService.CreateCourseRequest request = new()
        {
            Name = "",
        };

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("Course name cannot be empty.", exception.Message);

        _courseRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Course>()), Times.Never);
    }
}
