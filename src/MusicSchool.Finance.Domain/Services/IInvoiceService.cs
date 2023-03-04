using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Domain.Services;

public interface IInvoiceService
{
    Task<List<Invoice>> GenerateInvoicesAsync(GenerateInvoicesRequest request);

    public class GenerateInvoicesRequest
    {
        public Guid StudentId { get; init; }
    }
}
