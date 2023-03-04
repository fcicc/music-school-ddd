using System.Net;
using MusicSchool.Finance.Domain.External.SchoolManagement;
using MusicSchool.Finance.Domain.External.SchoolManagement.Models;

namespace MusicSchool.Finance.Infrastructure.External.SchoolManagement;

public class SchoolManagementClient : ISchoolManagementClient
{
    private readonly HttpClient _httpClient;

    public SchoolManagementClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
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

        return await response.Content.ReadAsAsync<CourseResponse>();
    }

    public async Task<List<EnrollmentResponse>> GetEnrollmentsAsync(Guid? studentId = null)
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

        return await response.Content.ReadAsAsync<List<EnrollmentResponse>>();
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

        return await response.Content.ReadAsAsync<StudentResponse>();
    }
}
