using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;

namespace MusicSchool.Finance.Domain.Services.Internal;

internal class InvoicePaymentFactory
    : AbstractTransactionFactory<
        ITransactionService.CreateInvoicePaymentRequest,
        InvoicePayment>
{
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IRepository<InvoicePayment> _invoicePaymentRepository;

    public InvoicePaymentFactory(
        IRepository<Invoice> invoiceRepository,
        IRepository<InvoicePayment> invoicePaymentRepository)
    {
        _invoiceRepository = invoiceRepository;
        _invoicePaymentRepository = invoicePaymentRepository;
    }

    protected override async Task<InvoicePayment> CreateAsync(
        ITransactionService.CreateInvoicePaymentRequest request)
    {
        if (request.Value < 0)
        {
            throw new DomainException("Value cannot be less than zero.");
        }

        bool hasInvoice = await _invoiceRepository
            .AsQueryable()
            .AnyAsync(i => i.Id == request.InvoiceId);

        if (!hasInvoice)
        {
            throw new DomainException("Invoice not found.");
        }

        bool isPaidInvoice = await _invoicePaymentRepository
            .AsQueryable()
            .AnyAsync(p => p.InvoiceId == request.InvoiceId);

        if (isPaidInvoice)
        {
            throw new DomainException("This invoice is already paid.");
        }

        return new()
        {
            Id = Guid.NewGuid(),
            Date = request.Date,
            Value = request.Value,
            InvoiceId = request.InvoiceId,
        };
    }
}
