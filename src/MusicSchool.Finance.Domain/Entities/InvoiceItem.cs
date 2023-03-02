using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Entities;

public class InvoiceItem
{
    internal InvoiceItem() { }

    public Guid EnrollmentId { get; init; }

    public Guid CourseId { get; init; }

    public string CourseName { get; init; } = "";

    public BrlAmount Value { get; init; }
}
