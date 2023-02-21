using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Factories;

public interface ICourseFactory
{
    Course CreateCourse(string name);
}
