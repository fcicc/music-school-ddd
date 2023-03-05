using System.Text.Json;
using MusicSchool.SchoolManagement.Infrastructure.Json.Converters;

namespace MusicSchool.SchoolManagement.Infrastructure.Json;

public static class JsonConfigurationHelper
{
    public static JsonSerializerOptions ConfigureJsonSerializerOptions(
        JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

        options.Converters.Add(new BrlAmountJsonConverter());
        options.Converters.Add(new DateMonthOnlyJsonConverter());

        return options;
    }
}
