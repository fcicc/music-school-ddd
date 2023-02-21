using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Factories;

public interface IStudentFactory
{
    Student CreateStudent(string name);
}
