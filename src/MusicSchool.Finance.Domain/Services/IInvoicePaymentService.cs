using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Services;

public interface IInvoicePaymentService
{
    Task<InvoicePayment> PayInvoiceAsync(PayInvoiceRequest request);

    public class PayInvoiceRequest
    {
        public Guid InvoiceId { get; init; }

        public BrlAmount Value { get; init; }
    }
}
