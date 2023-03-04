using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MusicSchool.SchoolManagement.Domain.ValueObjects;

public readonly struct DateMonthOnly : IComparable<DateMonthOnly>, IEquatable<DateMonthOnly>
{
    private const string StringFormat = "yyyy-MM";

    private readonly DateTime _underlyingTimestamp;

    public DateMonthOnly(int year, int month)
    {
        _underlyingTimestamp = new DateTime(year, month, 1);
    }

    public DateMonthOnly(DateTime dateTime)
    {
        _underlyingTimestamp = new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    public int Year => _underlyingTimestamp.Year;

    public int Month => _underlyingTimestamp.Month;

    public static DateMonthOnly Parse(string s)
    {
        return new(DateTime.ParseExact(s, StringFormat, CultureInfo.InvariantCulture));
    }

    public static bool operator ==(DateMonthOnly x, DateMonthOnly y) =>
        x.Equals(y);

    public static bool operator !=(DateMonthOnly x, DateMonthOnly y) =>
        !x.Equals(y);

    public static bool operator >(DateMonthOnly x, DateMonthOnly y) =>
        x.CompareTo(y) > 0;

    public static bool operator <(DateMonthOnly x, DateMonthOnly y) =>
        x.CompareTo(y) < 0;

    public static bool operator >=(DateMonthOnly x, DateMonthOnly y) =>
        x.CompareTo(y) >= 0;

    public static bool operator <=(DateMonthOnly x, DateMonthOnly y) =>
        x.CompareTo(y) <= 0;

    public override bool Equals([NotNullWhen(true)] object? obj) =>
        obj is DateMonthOnly other ? Equals(other) : false;

    public int CompareTo(DateMonthOnly other) =>
        _underlyingTimestamp.CompareTo(other._underlyingTimestamp);

    public bool Equals(DateMonthOnly other) =>
        _underlyingTimestamp.Equals(other._underlyingTimestamp);

    public override int GetHashCode() => _underlyingTimestamp.GetHashCode();

    public override string ToString() =>
        _underlyingTimestamp.ToString(StringFormat, CultureInfo.InvariantCulture);

    public DateTime ToDateTime() => _underlyingTimestamp;
}
