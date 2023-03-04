namespace MusicSchool.Finance.Domain.Entities;

public class InvoicePayment : Transaction
{
    internal InvoicePayment() { }

    public Guid InvoiceId { get; init; }
}
