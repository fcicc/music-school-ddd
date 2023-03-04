using System.Text.Json;
using MusicSchool.Finance.Infrastructure.Json.Converters;

namespace MusicSchool.Finance.Infrastructure.Json;

public static class JsonConfigurationHelper
{
    public static JsonSerializerOptions ConfigureJsonSerializerOptions(
        JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

        options.Converters.Add(new BrlAmountJsonConverter());
        options.Converters.Add(new DateMonthOnlyJsonConverter());
        options.Converters.Add(new DateOnlyJsonConverter());

        return options;
    }
}
