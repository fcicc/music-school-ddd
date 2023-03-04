using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Domain.Services;

public interface IInvoiceService
{
    Task<List<Invoice>> GenerateInvoicesForStudentAsync(
        GenerateInvoicesForStudentRequest request
    );

    public class GenerateInvoicesForStudentRequest
    {
        public Guid StudentId { get; init; }
    }
}
