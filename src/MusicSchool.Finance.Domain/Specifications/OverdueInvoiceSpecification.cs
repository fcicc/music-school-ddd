using System.Linq.Expressions;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Specifications;

public class OverdueInvoiceSpecification : ISpecification<Invoice>
{
    private readonly DateMonthOnly _atMonth;
    private readonly IQueryable<Transaction> _transactions;

    public OverdueInvoiceSpecification(
        DateMonthOnly atMonth,
        IQueryable<Transaction> transactions)
    {
        _atMonth = atMonth;
        _transactions = transactions;
    }

    public Expression<Func<Invoice, bool>> AsPredicate()
    {
        return i =>
            !_transactions.Any(t =>
                t.GetType() == typeof(InvoicePayment) &&
                ((InvoicePayment)t).InvoiceId == i.Id
            ) &&
            i.Month < _atMonth;
    }
}
