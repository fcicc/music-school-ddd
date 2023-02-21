using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Factories;

public interface ICourseFactory
{
    Course CreateCourse(string name);
}
