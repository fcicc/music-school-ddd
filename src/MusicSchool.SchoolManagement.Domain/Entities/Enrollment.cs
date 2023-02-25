using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Entities;

public class Enrollment : IAggregateRoot
{
    internal Enrollment() { }

    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public Guid CourseId { get; init; }

    public DateMonthOnly StartMonth { get; init; }

    public DateMonthOnly EndMonth { get; init; }

    public BrlAmount MonthlyBillingValue { get; init; }
}
