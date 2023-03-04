using System.Linq.Expressions;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Specifications;

public class OverdueInvoiceSpecification : ISpecification<Invoice>
{
    private readonly DateMonthOnly _atMonth;
    public readonly IQueryable<InvoicePayment> _invoicePayments;

    public OverdueInvoiceSpecification(
        DateMonthOnly atMonth,
        IQueryable<InvoicePayment> invoicePayments)
    {
        _atMonth = atMonth;
        _invoicePayments = invoicePayments;
    }

    public Expression<Func<Invoice, bool>> AsPredicate()
    {
        return i =>
            !_invoicePayments.Any(p => p.InvoiceId == i.Id) &&
            i.Month < _atMonth;
    }
}
