namespace MusicSchool.SchoolManagement.Domain.Entities;

public class Allocation
{
    public Guid Id { get; init; }

    public Guid TeacherId { get; init; }

    public Guid EnrollmentId { get; init; }

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }
}
