using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Services;

public interface IEnrollmentService
{
    Task<Enrollment> EnrollAsync(EnrollRequest request);

    public class EnrollRequest
    {
        public Guid StudentId { get; init; }

        public Guid CourseId { get; init; }

        public DateMonthOnly StartMonth { get; init; }

        public DateMonthOnly EndMonth { get; init; }

        public BrlAmount MonthlyBillingValue { get; init; }
    }
}
