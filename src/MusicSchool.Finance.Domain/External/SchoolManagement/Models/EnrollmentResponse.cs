using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.External.SchoolManagement.Models;

public class EnrollmentResponse
{
    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public Guid CourseId { get; init; }

    public DateMonthOnly StartMonth { get; init; }

    public DateMonthOnly EndMonth { get; init; }

    public BrlAmount MonthlyBillingValue { get; init; }
}
