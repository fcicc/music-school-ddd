using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.ValueObjects;
using MusicSchool.SchoolManagement.Repositories;

namespace MusicSchool.SchoolManagement.Domain.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IRepository<Enrollment> _enrollmentRepository;

    public EnrollmentService(IRepository<Enrollment> enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<Enrollment> EnrollAsync(
        Guid studentId,
        Guid courseId,
        DateOnly startDate,
        DateOnly endDate,
        int lessonsPerMonth,
        BrlAmount monthlyBill)
    {
        if (startDate > endDate)
        {
            throw new DomainException("Enrollment start date cannot be after end date.");
        }
        if (lessonsPerMonth <= 0)
        {
            throw new DomainException("Enrollment lessons per month should be greater than zero.");
        }
        if (monthlyBill < 0)
        {
            throw new DomainException("Enrollment monthly bill cannot be less than zero.");
        }

        Enrollment enrollment = new()
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            CourseId = courseId,
            StartDate = startDate,
            EndDate = endDate,
            LessonsPerMonth = lessonsPerMonth,
            MonthlyBill = monthlyBill,
        };

        await _enrollmentRepository.AddAsync(enrollment);

        return enrollment;
    }
}
