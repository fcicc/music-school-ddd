using System.Text.Json;
using System.Text.Json.Serialization;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.ValueObjects.JsonConverters;

public class DateMonthOnlyJsonConverter : JsonConverter<DateMonthOnly>
{
    public override DateMonthOnly Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return DateMonthOnly.Parse(reader.GetString() ?? "");
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateMonthOnly value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
