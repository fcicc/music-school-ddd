using System.Net;
using MusicSchool.SchoolManagement.Client.Models.Response;

namespace MusicSchool.SchoolManagement.Client;

public class SchoolManagementClient : ISchoolManagementClient
{
    private readonly HttpClient _httpClient;

    public SchoolManagementClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<Course>> GetCoursesAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(
            "/courses"
        );
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsAsync<List<Course>>();
    }

    public async Task<Course?> GetCourseAsync(Guid id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(
            $"/courses/{id}"
        );
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsAsync<Course>();
    }

    public async Task<IReadOnlyList<Enrollment>> GetEnrollmentsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync(
            "/enrollments"
        );
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsAsync<List<Enrollment>>();
    }

    public async Task<Enrollment?> GetEnrollmentAsync(Guid id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(
            $"/enrollments/{id}"
        );
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsAsync<Enrollment>();
    }

    public async Task<IReadOnlyList<Student>> GetStudentsAsync(bool activeOnly = false)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(
            $"/students?{nameof(activeOnly)}={activeOnly}"
        );
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsAsync<List<Student>>();
    }

    public async Task<Student?> GetStudentAsync(Guid id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(
            $"/students/{id}"
        );
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsAsync<Student>();
    }
}
