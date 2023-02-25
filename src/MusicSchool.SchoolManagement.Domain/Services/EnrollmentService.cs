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
        DateOnly startDate,
        DateOnly endDate,
        BrlAmount monthlyBill)
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

        if (startDate > endDate)
        {
            throw new DomainException("Start date cannot be after end date.");
        }

        if (monthlyBill < 0)
        {
            throw new DomainException("Monthly bill cannot be less than zero.");
        }

        List<Enrollment> overlappingEnrollments = await _enrollmentRepository
            .FindAsync(new OverlappingEnrollmentSpecification(
                studentId,
                courseId,
                startDate,
                endDate
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
            StartDate = startDate,
            EndDate = endDate,
            MonthlyBill = monthlyBill,
        };

        await _enrollmentRepository.AddAsync(enrollment);

        return enrollment;
    }
}
