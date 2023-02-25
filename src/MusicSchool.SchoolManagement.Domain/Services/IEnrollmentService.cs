using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Services;

public interface IEnrollmentService
{
    Task<Enrollment> EnrollAsync(
        Guid studentId,
        Guid courseId,
        DateOnly startDate,
        DateOnly endDate,
        BrlAmount monthlyBill);
}
