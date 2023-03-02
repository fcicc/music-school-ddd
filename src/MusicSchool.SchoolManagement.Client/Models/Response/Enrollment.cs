namespace MusicSchool.SchoolManagement.Client.Models.Response;

public class Enrollment
{
    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public Guid CourseId { get; init; }

    public string StartMonth { get; init; } = "";

    public string EndMonth { get; init; } = "";

    public decimal MonthlyBillingValue { get; init; }
}
