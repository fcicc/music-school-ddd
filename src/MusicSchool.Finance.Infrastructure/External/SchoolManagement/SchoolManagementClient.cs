using System.Net;
using System.Text.Json;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.External.SchoolManagement;
using MusicSchool.Finance.Domain.External.SchoolManagement.Models;

namespace MusicSchool.Finance.Infrastructure.External.SchoolManagement;

public class SchoolManagementClient : ISchoolManagementClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public SchoolManagementClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    public async Task<CourseResponse?> GetCourseAsync(Guid id)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(
            $"/courses/{id}"
        );
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        string payload = await response.Content.ReadAsStringAsync();

        return
            JsonSerializer.Deserialize<CourseResponse>(
                payload,
                _jsonSerializerOptions
            )
            ?? throw new InconsistentExternalStateException(
                "Unexpected situation: course should not be null."
            );
    }

    public async Task<IReadOnlyList<EnrollmentResponse>> GetEnrollmentsAsync(Guid? studentId = null)
    {
        List<string> queryParams = new();

        if (studentId != null)
        {
            queryParams.Add($"studentId={studentId.Value.ToString()}");
        }

        using HttpResponseMessage response = await _httpClient.GetAsync(
             $"/enrollments?{string.Join('&', queryParams.ToArray())}"
        );
        response.EnsureSuccessStatusCode();

        string payload = await response.Content.ReadAsStringAsync();

        return
            JsonSerializer.Deserialize<List<EnrollmentResponse>>(
                payload,
                _jsonSerializerOptions
            )
            ?? throw new InconsistentExternalStateException(
                "Unexpected situation: enrollment list should not be null."
            );
    }

    public async Task<StudentResponse?> GetStudentAsync(Guid id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(
            $"/students/{id}"
        );
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        string payload = await response.Content.ReadAsStringAsync();

        return
            JsonSerializer.Deserialize<StudentResponse>(
                payload,
                _jsonSerializerOptions
            )
            ?? throw new InconsistentExternalStateException(
                "Unexpected situation: student should not be null."
            );
    }
}
