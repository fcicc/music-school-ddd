using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Factories;

public interface IEnrollmentFactory
{
    Enrollment CreateEnrollment(
        Student student,
        Course course,
        DateOnly startDate,
        DateOnly endDate,
        int lessonsPerMonth,
        BrlAmount monthlyBill
    );
}
