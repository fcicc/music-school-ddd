using Moq;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Services;
using MusicSchool.SchoolManagement.Domain.Specifications;

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

        _studentRepositoryMock
            .Setup(r => r.FindAsync(
                It.Is<StudentNameSpecification>(s => s.Name == validName)
            ))
            .ReturnsAsync(new List<Student>());

        Student student = await _sut.CreateAsync(validName);

        Assert.NotEqual(Guid.Empty, student.Id);
        Assert.Equal(validName, student.Name);

        _studentRepositoryMock.Verify(r => r.AddAsync(student), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithExistingName_ThrowsDomainException()
    {
        const string validName = "Luiz Melodia";

        _studentRepositoryMock
            .Setup(r => r.FindAsync(
                It.Is<StudentNameSpecification>(s => s.Name == validName)
            ))
            .ReturnsAsync(new List<Student>
            {
                new Student
                {
                    Id = Guid.NewGuid(),
                    Name = validName,
                },
            });

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(validName)
        );

        Assert.Equal("Student with same name already exists.", exception.Message);

        _studentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Student>()), Times.Never);
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
