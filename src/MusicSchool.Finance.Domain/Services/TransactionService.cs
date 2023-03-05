using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;

namespace MusicSchool.Finance.Domain.Services;

public class TransactionService : ITransactionService
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IRepository<Invoice> _invoiceRepository;

    public TransactionService(
        IRepository<Transaction> transactionRepository,
        IRepository<Invoice> invoiceRepository)
    {
        _transactionRepository = transactionRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<Transaction> CreateAsync(
        ITransactionService.CreateTransactionRequest request)
    {
        if (request.Value < 0)
        {
            throw new DomainException("Value cannot be less than zero.");
        }

        if (request is ITransactionService.CreateExtraPaymentRequest createExtraPaymentRequest)
        {
            return await CreateExtraPaymentAsync(createExtraPaymentRequest);
        }

        if (request is ITransactionService.CreateInvoicePaymentRequest createInvoicePaymentRequest)
        {
            return await CreateInvoicePaymentAsync(createInvoicePaymentRequest);
        }

        throw new DomainException("Unknown transaction type.");
    }

    private async Task<Transaction> CreateExtraPaymentAsync(
        ITransactionService.CreateExtraPaymentRequest request)
    {
        if (string.IsNullOrEmpty(request.Description))
        {
            throw new DomainException("Description should not be empty.");
        }

        ExtraPayment extraPayment = new()
        {
            Id = Guid.NewGuid(),
            Date = request.Date,
            Value = request.Value,
            Description = request.Description,
        };

        await _transactionRepository.AddAsync(extraPayment);

        return extraPayment;
    }

    private async Task<Transaction> CreateInvoicePaymentAsync(
        ITransactionService.CreateInvoicePaymentRequest request)
    {
        bool hasInvoice = await _invoiceRepository
            .AsQueryable()
            .AnyAsync(i => i.Id == request.InvoiceId);

        if (!hasInvoice)
        {
            throw new DomainException("Invoice not found.");
        }

        bool isPaidInvoice = await _transactionRepository
            .AsQueryable()
            .AnyAsync(t =>
                t.GetType() == typeof(InvoicePayment) &&
                ((InvoicePayment)t).InvoiceId == request.InvoiceId
            );

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