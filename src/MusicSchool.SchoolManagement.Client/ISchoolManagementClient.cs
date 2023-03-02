using MusicSchool.SchoolManagement.Client.Models.Response;

namespace MusicSchool.SchoolManagement.Client;

public interface ISchoolManagementClient
{
    Task<IReadOnlyList<Course>> GetCoursesAsync();

    Task<Course?> GetCourseAsync(Guid id);

    Task<IReadOnlyList<Enrollment>> GetEnrollmentsAsync();

    Task<Enrollment?> GetEnrollmentAsync(Guid id);

    Task<IReadOnlyList<Student>> GetStudentsAsync(bool activeOnly = false);

    Task<Student?> GetStudentAsync(Guid id);
}
