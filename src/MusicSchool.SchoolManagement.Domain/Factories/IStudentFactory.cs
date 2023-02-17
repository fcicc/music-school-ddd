using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Factories;

public interface IStudentFactory
{
    Student CreateStudent(string name);
}
