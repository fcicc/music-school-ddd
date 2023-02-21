using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Entities;

public class Enrollment : IAggregateRoot
{
    internal Enrollment() { }

    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public Guid CourseId { get; init; }

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }

    public int LessonsPerMonth { get; init; }

    public BrlAmount MonthlyBill { get; init; }
}
