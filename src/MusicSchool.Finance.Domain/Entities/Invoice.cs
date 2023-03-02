using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Entities;

public class Invoice : IAggregateRoot
{
    public Guid Id { get; init; }

    public Guid StudentId { get; init; }

    public IReadOnlyList<InvoiceItem> Items { get; init; } = new InvoiceItem[] { };

    public BrlAmount TotalValue { get; init; }
}
