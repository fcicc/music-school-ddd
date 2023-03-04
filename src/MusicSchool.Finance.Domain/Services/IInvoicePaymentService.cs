using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Services;

public interface IInvoicePaymentService
{
    Task<InvoicePayment> CreateAsync(CreateInvoicePaymentRequest request);

    public class CreateInvoicePaymentRequest
    {
        public Guid InvoiceId { get; init; }

        public DateOnly Date { get; init; }

        public BrlAmount Value { get; init; }
    }
}
