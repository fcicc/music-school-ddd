using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Factories;

namespace MusicSchool.SchoolManagement.Domain.Tests.Factories;

public class CourseFactoryTests
{
    private readonly CourseFactory _sut = new CourseFactory();

    [Fact]
    public void CreateCourse_WithValidName_ReturnsNewCourse()
    {
        const string validName = "Técnica Vocal";

        Course course = _sut.CreateCourse(validName);

        Assert.NotEqual(Guid.Empty, course.Id);
        Assert.Equal(validName, course.Name);
    }

    [Fact]
    public void CreateCourse_WithEmptyName_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => _sut.CreateCourse(""));
    }
}
