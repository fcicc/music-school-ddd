using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.External.SchoolManagement;
using MusicSchool.Finance.Domain.External.SchoolManagement.Models.Response;

namespace MusicSchool.Finance.Domain.Services;

public class InvoiceService : IInvoiceService
{
    private readonly ISchoolManagementClient _schoolManagementClient;

    public InvoiceService(ISchoolManagementClient schoolManagementClient)
    {
        _schoolManagementClient = schoolManagementClient;
    }

    public async Task<IReadOnlyList<Invoice>> GenerateInvoicesForStudent(Guid studentId)
    {
        Student? student = await _schoolManagementClient.GetStudentAsync(studentId);
        if (student == null)
        {
            throw new DomainException("Student not found.");
        }

        throw new NotImplementedException();
    }
}
