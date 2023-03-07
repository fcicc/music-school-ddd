using System.Text.Json.Serialization;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Services;

public interface ITransactionService
{
    Task<Transaction> CreateAsync(CreateTransactionRequest request);

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(CreateExtraPaymentRequest), "ExtraPayment")]
    [JsonDerivedType(typeof(CreateInvoicePaymentRequest), "InvoicePayment")]
    public abstract class CreateTransactionRequest
    {
        public DateOnly Date { get; init; }

        public BrlAmount Value { get; init; }
    }

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(CreateExtraPaymentRequest), "ExtraPayment")]
    public class CreateExtraPaymentRequest : CreateTransactionRequest
    {
        public string Description { get; init; } = "";
    }

    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(CreateInvoicePaymentRequest), "InvoicePayment")]
    public class CreateInvoicePaymentRequest : CreateTransactionRequest
    {
        public Guid InvoiceId { get; init; }
    }
}
