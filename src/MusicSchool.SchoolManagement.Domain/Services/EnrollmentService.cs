using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Specifications;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IRepository<Enrollment> _enrollmentRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Course> _courseRepository;

    public EnrollmentService(
        IRepository<Enrollment> enrollmentRepository,
        IRepository<Student> studentRepository,
        IRepository<Course> courseRepository)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<Enrollment> EnrollAsync(
        Guid studentId,
        Guid courseId,
        DateMonthOnly startMonth,
        DateMonthOnly endMonth,
        BrlAmount monthlyBillingValue)
    {
        Student? student = await _studentRepository.FindOneAsync(studentId);
        if (student == null)
        {
            throw new DomainException("Student not found.");
        }

        Course? course = await _courseRepository.FindOneAsync(courseId);
        if (course == null)
        {
            throw new DomainException("Course not found.");
        }

        if (startMonth > endMonth)
        {
            throw new DomainException("Start month cannot be after end month.");
        }

        if (monthlyBillingValue < 0)
        {
            throw new DomainException("Monthly billing value cannot be less than zero.");
        }

        List<Enrollment> overlappingEnrollments = await _enrollmentRepository
            .FindAsync(new OverlappingEnrollmentSpecification(
                studentId,
                courseId,
                startMonth,
                endMonth
            ));
        if (overlappingEnrollments.Any())
        {
            throw new DomainException("There already exists an enrollment that overlaps the new one.");
        }

        Enrollment enrollment = new()
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            CourseId = courseId,
            StartMonth = startMonth,
            EndMonth = endMonth,
            MonthlyBillingValue = monthlyBillingValue,
        };

        await _enrollmentRepository.AddAsync(enrollment);

        return enrollment;
    }
}
