using System.Diagnostics.CodeAnalysis;

namespace MusicSchool.SchoolManagement.Domain.ValueObjects;

public struct BrlAmount : IComparable<BrlAmount>, IEquatable<BrlAmount>
{
    public decimal Value { get; set; }

    public static implicit operator BrlAmount(decimal value) =>
        new BrlAmount { Value = value };

    public static implicit operator decimal(BrlAmount brlAmount) =>
        brlAmount.Value;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is BrlAmount other ? Equals(other) : false;

    public int CompareTo(BrlAmount other) => Value.CompareTo(other.Value);

    public bool Equals(BrlAmount other) => Value.Equals(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"BRL {Value}";
}
