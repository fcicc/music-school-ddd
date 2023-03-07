using System.Text.Json.Serialization;

namespace MusicSchool.Finance.Domain.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ExtraPayment), "ExtraPayment")]
public class ExtraPayment : Transaction
{
    internal ExtraPayment() { }

    public string Description { get; init; } = "";
}
