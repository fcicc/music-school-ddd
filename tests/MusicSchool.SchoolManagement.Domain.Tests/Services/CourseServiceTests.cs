using Moq;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Repositories;

namespace MusicSchool.SchoolManagement.Domain.Tests.Services;

public class CourseServiceTests
{
    private readonly Mock<IRepository<Course>> _courseRepositoryMock;

    private readonly CourseService _sut;

    public CourseServiceTests()
    {
        _courseRepositoryMock = new Mock<IRepository<Course>>();

        _sut = new CourseService(_courseRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidName_ReturnsNewCourse()
    {
        const string validName = "Técnica Vocal";

        Course course = await _sut.CreateAsync(validName);

        Assert.NotEqual(Guid.Empty, course.Id);
        Assert.Equal(validName, course.Name);

        _courseRepositoryMock.Verify(r => r.AddAsync(course), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyName_ThrowsDomainException()
    {
        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync("")
        );

        Assert.Equal("Course name cannot be empty.", exception.Message);

        _courseRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Course>()), Times.Never);
    }
}
