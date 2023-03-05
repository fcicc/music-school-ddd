using System.Text.Json.Serialization;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ExtraPayment), "ExtraPayment")]
[JsonDerivedType(typeof(InvoicePayment), "InvoicePayment")]
public abstract class Transaction : IAggregateRoot
{
    internal Transaction() { }

    public Guid Id { get; init; }

    public DateOnly Date { get; init; }

    public BrlAmount Value { get; init; }
}
