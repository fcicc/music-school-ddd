using System.Net;
using MusicSchool.Finance.Domain.External.SchoolManagement;
using MusicSchool.Finance.Domain.External.SchoolManagement.Models.Response;

namespace MusicSchool.Finance.Infrastructure.External.SchoolManagement;

public class SchoolManagementClient : ISchoolManagementClient
{
    private readonly HttpClient _httpClient;

    public SchoolManagementClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
