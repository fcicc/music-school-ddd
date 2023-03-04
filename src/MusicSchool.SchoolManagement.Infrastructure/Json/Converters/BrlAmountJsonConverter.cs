using System.Text.Json;
using System.Text.Json.Serialization;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Infrastructure.Json.Converters;

public class BrlAmountJsonConverter : JsonConverter<BrlAmount>
{
    public override BrlAmount Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return new BrlAmount(reader.GetDecimal());
    }

    public override void Write(
        Utf8JsonWriter writer,
        BrlAmount value,
        JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}
