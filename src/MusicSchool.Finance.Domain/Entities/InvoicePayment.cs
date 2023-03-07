using System.Text.Json.Serialization;

namespace MusicSchool.Finance.Domain.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(InvoicePayment), "InvoicePayment")]
public class InvoicePayment : Transaction
{
    internal InvoicePayment() { }

    public Guid InvoiceId { get; init; }
}
