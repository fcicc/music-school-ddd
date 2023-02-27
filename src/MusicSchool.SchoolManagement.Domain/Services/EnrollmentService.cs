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

    public async Task<Enrollment> EnrollAsync(IEnrollmentService.EnrollRequest request)
    {
        Student? student = await _studentRepository.FindOneAsync(request.StudentId);
        if (student == null)
        {
            throw new DomainException("Student not found.");
        }

        Course? course = await _courseRepository.FindOneAsync(request.CourseId);
        if (course == null)
        {
            throw new DomainException("Course not found.");
        }

        if (request.StartMonth > request.EndMonth)
        {
            throw new DomainException("Start month cannot be after end month.");
        }

        if (request.MonthlyBillingValue < 0)
        {
            throw new DomainException("Monthly billing value cannot be less than zero.");
        }

        List<Enrollment> overlappingEnrollments = await _enrollmentRepository
            .FindAsync(new OverlappingEnrollmentSpecification(
                request.StudentId,
                request.CourseId,
                request.StartMonth,
                request.EndMonth
            ));
        if (overlappingEnrollments.Any())
        {
            throw new DomainException("There already exists an enrollment that overlaps the new one.");
        }

        Enrollment enrollment = new()
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            StartMonth = request.StartMonth,
            EndMonth = request.EndMonth,
            MonthlyBillingValue = request.MonthlyBillingValue,
        };

        await _enrollmentRepository.AddAsync(enrollment);

        return enrollment;
    }
}
