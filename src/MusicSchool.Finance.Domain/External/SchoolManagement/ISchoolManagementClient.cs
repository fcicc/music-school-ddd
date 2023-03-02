using MusicSchool.Finance.Domain.External.SchoolManagement.Models.Response;

namespace MusicSchool.Finance.Domain.External.SchoolManagement;

public interface ISchoolManagementClient
{
    Task<Student?> GetStudentAsync(Guid id);
}
