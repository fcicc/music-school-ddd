using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Services;

public interface ITransactionService
{
    Task<Transaction> CreateAsync(CreateTransactionRequest request);

    public abstract class CreateTransactionRequest
    {
        public DateOnly Date { get; init; }

        public BrlAmount Value { get; init; }
    }

    public class CreateExtraPaymentRequest : CreateTransactionRequest
    {
        public string Description { get; init; } = "";
    }

    public class CreateInvoicePaymentRequest : CreateTransactionRequest
    {
        public Guid InvoiceId { get; init; }
    }
}
