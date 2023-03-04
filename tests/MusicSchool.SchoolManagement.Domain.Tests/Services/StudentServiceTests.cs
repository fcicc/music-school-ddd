using MockQueryable.Moq;
using Moq;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;

namespace MusicSchool.SchoolManagement.Domain.Tests.Services;

public class StudentServiceTests
{
    private readonly Mock<IRepository<Student>> _studentRepositoryMock;

    private readonly StudentService _sut;

    public StudentServiceTests()
    {
        _studentRepositoryMock = new();

        _sut = new(_studentRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidName_ReturnsNewStudent()
    {
        IStudentService.CreateStudentRequest request = new()
        {
            Name = "Luiz Melodia",
        };

        _studentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Student>().BuildMock());

        Student student = await _sut.CreateAsync(request);

        Assert.NotEqual(Guid.Empty, student.Id);
        Assert.Equal(request.Name, student.Name);

        _studentRepositoryMock.Verify(r => r.AddAsync(student), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithExistingName_ThrowsDomainException()
    {
        IStudentService.CreateStudentRequest request = new()
        {
            Name = "Luiz Melodia",
        };

        _studentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Student>
            {
                new()
                {
                    Name = request.Name,
                },
            }.BuildMock());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("Student with same name already exists.", exception.Message);

        _studentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyName_ThrowsDomainException()
    {
        IStudentService.CreateStudentRequest request = new()
        {
            Name = "",
        };

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("Student name cannot be empty.", exception.Message);

        _studentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Never);
    }
}
