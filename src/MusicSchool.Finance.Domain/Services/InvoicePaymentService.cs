using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;

namespace MusicSchool.Finance.Domain.Services;

public class InvoicePaymentService : IInvoicePaymentService
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<Invoice> _invoiceRepository;

    public InvoicePaymentService(
        IRepository<Transaction> transactionRepository,
        IRepository<Invoice> invoiceRepository)
    {
        _transactionRepository = transactionRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<InvoicePayment> CreateAsync(
        IInvoicePaymentService.CreateInvoicePaymentRequest request)
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

        bool isPaidInvoice = await _transactionRepository
            .AsQueryable()
            .OfType<InvoicePayment>()
            .AnyAsync(p => p.InvoiceId == request.InvoiceId);

        if (isPaidInvoice)
        {
            throw new DomainException("This invoice is already paid.");
        }

        InvoicePayment invoicePayment = new()
        {
            Id = Guid.NewGuid(),
            Date = request.Date,
            Value = request.Value,
            InvoiceId = request.InvoiceId,
        };

        await _transactionRepository.AddAsync(invoicePayment);

        return invoicePayment;
    }
}