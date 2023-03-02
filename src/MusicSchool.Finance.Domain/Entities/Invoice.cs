using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Entities;

public class Invoice : IAggregateRoot
{
    internal Invoice() { }

    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public string StudentName { get; init; } = "";

    public DateMonthOnly Month { get; init; }

    public IReadOnlyList<InvoiceItem> Items { get; init; } = new InvoiceItem[] { };

    public BrlAmount TotalValue { get; init; }
}
