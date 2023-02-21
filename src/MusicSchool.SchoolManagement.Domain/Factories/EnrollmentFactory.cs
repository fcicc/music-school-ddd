using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Exceptions;
using MusicSchool.SchoolManagement.Domain.ValueObjects;

namespace MusicSchool.SchoolManagement.Domain.Factories;

public class EnrollmentFactory : IEnrollmentFactory
{
    public Enrollment CreateEnrollment(
        Student student,
        Course course,
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

        return new Enrollment
        {
            Id = Guid.NewGuid(),
            StudentId = student.Id,
            CourseId = course.Id,
            StartDate = startDate,
            EndDate = endDate,
            LessonsPerMonth = lessonsPerMonth,
            MonthlyBill = monthlyBill,
        };
    }
}
