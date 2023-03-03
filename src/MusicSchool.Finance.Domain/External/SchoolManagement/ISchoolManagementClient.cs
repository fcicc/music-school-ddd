using MusicSchool.Finance.Domain.External.SchoolManagement.Models;

namespace MusicSchool.Finance.Domain.External.SchoolManagement;

public interface ISchoolManagementClient
{
    Task<CourseResponse?> GetCourseAsync(Guid id);

    Task<IReadOnlyList<EnrollmentResponse>> GetEnrollmentsAsync(Guid? studentId = null);

    Task<StudentResponse?> GetStudentAsync(Guid id);
}
