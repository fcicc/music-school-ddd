using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;

namespace MusicSchool.Finance.Domain.Services;

public class InvoicePaymentService : IInvoicePaymentService
{
    private readonly IRepository<InvoicePayment> _invoicePaymentRepository;
    private readonly IRepository<Invoice> _invoiceRepository;

    public InvoicePaymentService(
        IRepository<InvoicePayment> invoicePaymentRepository,
        IRepository<Invoice> invoiceRepository)
    {
        _invoicePaymentRepository = invoicePaymentRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<InvoicePayment> PayInvoiceAsync(
        IInvoicePaymentService.PayInvoiceRequest request)
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

        InvoicePayment invoicePayment = new()
        {
            Id = Guid.NewGuid(),
            Value = request.Value,
            InvoiceId = request.InvoiceId,
        };

        await _invoicePaymentRepository.AddAsync(invoicePayment);

        return invoicePayment;
    }
}