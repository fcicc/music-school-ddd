using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MusicSchool.SchoolManagement.Domain.ValueObjects;

public struct DateMonthOnly : IComparable<DateMonthOnly>, IEquatable<DateMonthOnly>
{
    public DateMonthOnly()
    {
        Year = 1;
        Month = 1;
    }

    public DateMonthOnly(int year, int month)
    {
        Year = year;
        Month = month;
    }

    public int Year { get; set; }

    public int Month { get; set; }

    public static DateMonthOnly Parse(string s)
    {
        DateTime dateTime = DateTime.ParseExact(s, "YYYY-MM", CultureInfo.InvariantCulture);
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
        ToDateTime().ToString("yyyy-MM", CultureInfo.InvariantCulture);

    public DateTime ToDateTime() => new(Year, Month, 1);
}
