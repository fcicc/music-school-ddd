using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Domain.Services;

public interface IInvoiceService
{
    Task<IReadOnlyList<Invoice>> GenerateInvoicesForStudent(Guid studentId);
}
