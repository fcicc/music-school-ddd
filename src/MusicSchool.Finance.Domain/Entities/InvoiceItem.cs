using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Entities;

public class InvoiceItem
{
    public Guid EnrollmentId { get; init; }

    public BrlAmount Value { get; init; }
}
