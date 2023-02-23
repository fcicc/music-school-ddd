using Moq;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Repositories;

namespace MusicSchool.SchoolManagement.Domain.Tests.Services;

public class StudentServiceTests
{
    private readonly Mock<IRepository<Student>> _studentRepositoryMock;

    private readonly StudentService _sut;

    public StudentServiceTests()
    {
        _studentRepositoryMock = new Mock<IRepository<Student>>();

        _sut = new StudentService(_studentRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidName_ReturnsNewStudent()
    {
        const string validName = "Luiz Melodia";

        Student student = await _sut.CreateAsync(validName);

        Assert.NotEqual(Guid.Empty, student.Id);
        Assert.Equal(validName, student.Name);

        _studentRepositoryMock.Verify(r => r.AddAsync(student), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyName_ThrowsDomainException()
    {
        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync("")
        );

        Assert.Equal("Student name cannot be empty.", exception.Message);

        _studentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Never);
    }
}
