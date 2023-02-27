using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;
using MusicSchool.SchoolManagement.Domain.ValueObjects.Converters;

namespace MusicSchool.SchoolManagement.Domain.ValueObjects;

[JsonConverter(typeof(DateMonthOnlyJsonConverter))]
public struct DateMonthOnly : IComparable<DateMonthOnly>, IEquatable<DateMonthOnly>
{
    private const string StringFormat = "yyyy-MM";

    public DateMonthOnly(int year, int month)
    {
        Year = year;
        Month = month;
    }

    public int Year { get; set; }

    public int Month { get; set; }

    public static DateMonthOnly Parse(string s)
    {
        DateTime dateTime = DateTime.ParseExact(s, StringFormat, CultureInfo.InvariantCulture);
        return new(dateTime.Year, dateTime.Month);
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
        ToDateTime().CompareTo(other.ToDateTime());

    public bool Equals(DateMonthOnly other) =>
        ToDateTime().Equals(other.ToDateTime());

    public override int GetHashCode() => ToDateTime().GetHashCode();

    public override string ToString() =>
        ToDateTime().ToString(StringFormat, CultureInfo.InvariantCulture);

    public DateTime ToDateTime() => new(Year, Month, 1);
}
