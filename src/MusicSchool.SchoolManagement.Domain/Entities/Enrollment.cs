namespace MusicSchool.SchoolManagement.Domain.Entities;

public class Enrollment : IAggregateRoot
{
    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public Guid CourseId { get; init; }

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }
}
