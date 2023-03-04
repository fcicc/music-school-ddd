using System.Text.Json;
using MusicSchool.Finance.Domain.Exceptions;

namespace MusicSchool.Finance.Infrastructure.External.SchoolManagement;

internal static class HttpContentExtensions
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

    public static async Task<TValue> ReadAsAsync<TValue>(this HttpContent content)
    {
        string payload = await content.ReadAsStringAsync();

        return
            JsonSerializer.Deserialize<TValue>(payload, _jsonSerializerOptions)
            ?? throw new InconsistentExternalStateException(
                "Unexpected situation: value should not be null."
            );
    }
}
