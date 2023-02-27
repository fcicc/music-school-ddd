using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MusicSchool.SchoolManagement.Domain.ValueObjects.Converters;

namespace MusicSchool.SchoolManagement.Domain.ValueObjects;

[JsonConverter(typeof(BrlAmountJsonConverter))]
public readonly struct BrlAmount : IComparable<BrlAmount>, IEquatable<BrlAmount>
{
    public BrlAmount(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public static implicit operator BrlAmount(decimal value) =>
        new BrlAmount(value);

    public static implicit operator decimal(BrlAmount brlAmount) =>
        brlAmount.Value;

    public static bool operator ==(BrlAmount x, BrlAmount y) =>
        x.Equals(y);

    public static bool operator !=(BrlAmount x, BrlAmount y) =>
        !x.Equals(y);

    public static bool operator >(BrlAmount x, BrlAmount y) =>
        x.CompareTo(y) > 0;

    public static bool operator <(BrlAmount x, BrlAmount y) =>
        x.CompareTo(y) < 0;

    public static bool operator >=(BrlAmount x, BrlAmount y) =>
        x.CompareTo(y) >= 0;

    public static bool operator <=(BrlAmount x, BrlAmount y) =>
        x.CompareTo(y) <= 0;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is BrlAmount other ? Equals(other) : false;

    public int CompareTo(BrlAmount other) => Value.CompareTo(other.Value);

    public bool Equals(BrlAmount other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"BRL {Value}";
}
