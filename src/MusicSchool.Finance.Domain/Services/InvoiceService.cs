using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.External.SchoolManagement;
using MusicSchool.Finance.Domain.External.SchoolManagement.Models;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Services;

public class InvoiceService : IInvoiceService
{
    private readonly ISchoolManagementClient _schoolManagementClient;
    private readonly IRepository<Invoice> _invoiceRepository;

    public InvoiceService(
        ISchoolManagementClient schoolManagementClient,
        IRepository<Invoice> invoiceRepository)
    {
        _schoolManagementClient = schoolManagementClient;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<List<Invoice>> GenerateInvoicesForStudentAsync(
        IInvoiceService.GenerateInvoicesForStudentRequest request)
    {
        StudentResponse? student = await _schoolManagementClient.GetStudentAsync(request.StudentId);
        if (student == null)
        {
            throw new DomainException("Student not found.");
        }

        IReadOnlyList<EnrollmentResponse> enrollments =
            await _schoolManagementClient.GetEnrollmentsAsync(student.Id);

        // Group enrollments by month
        Dictionary<DateMonthOnly, List<EnrollmentResponse>> enrollmentsByMonth =
            enrollments
                .SelectMany(
                    e => IterateMonths(e.StartMonth, e.EndMonth)
                        .Select(m => new
                        {
                            Enrollment = e,
                            Month = m,
                        })
                )
                .GroupBy(em => em.Month)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(em => em.Enrollment).ToList()
                );

        // Generate invoices by iterating over grouped months
        List<Invoice> invoices = new();

        Dictionary<Guid, CourseResponse> courseCache = new();
        foreach (KeyValuePair<DateMonthOnly, List<EnrollmentResponse>> kv in enrollmentsByMonth)
        {
            // Generate invoice items
            List<InvoiceItem> invoiceItems = new();
            foreach (EnrollmentResponse enrollment in kv.Value)
            {
                CourseResponse? course;
                if (!courseCache.TryGetValue(enrollment.CourseId, out course))
                {
                    course = await _schoolManagementClient.GetCourseAsync(enrollment.CourseId);
                    if (course == null)
                    {
                        throw new InconsistentExternalStateException(
                            "Unexpected situation: course not found for enrollment."
                        );
                    }

                    courseCache.Add(course.Id, course);
                }

                invoiceItems.Add(new()
                {
                    EnrollmentId = enrollment.Id,
                    CourseId = course.Id,
                    CourseName = course.Name,
                    Value = enrollment.MonthlyBillingValue,
                });
            }

            // Generate invoice by totalizing the items
            invoices.Add(new()
            {
                Id = Guid.NewGuid(),
                StudentId = student.Id,
                StudentName = student.Name,
                Month = kv.Key,
                Items = invoiceItems.OrderBy(i => i.CourseName).ToList(),
                TotalValue = invoiceItems.Sum(i => i.Value),
            });
        }

        // Order invoices by month
        invoices = invoices.OrderBy(i => i.Month).ToList();

        await _invoiceRepository.AddRangeAsync(invoices.ToArray());

        return invoices;
    }

    private IEnumerable<DateMonthOnly> IterateMonths(DateMonthOnly startMonth, DateMonthOnly endMonth)
    {
        DateMonthOnly currentMonth = startMonth;
        while (currentMonth <= endMonth)
        {
            yield return currentMonth;
            currentMonth = currentMonth.AddMonths(1);
        }
    }
}
