using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Exceptions;
using MusicSchool.SchoolManagement.Factories;

namespace MusicSchool.SchoolManagement.Domain.Tests.Factories;

public class StudentFactoryTests
{
    private readonly StudentFactory _sut = new StudentFactory();

    [Fact]
    public void CreateStudent_WithValidName_ReturnsNewStudent()
    {
        const string validName = "Luiz Melodia";

        Student student = _sut.CreateStudent(validName);

        Assert.NotEqual(Guid.Empty, student.Id);
        Assert.Equal(validName, student.Name);
    }

    [Fact]
    public void CreateStudent_WithEmptyName_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => _sut.CreateStudent(""));
    }
}