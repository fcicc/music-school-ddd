using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Api.Controllers;

[ApiController]
[Route("/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IInvoicePaymentService _invoicePaymentService;

    public TransactionsController(
        IRepository<Transaction> transactionRepository,
        IInvoicePaymentService invoicePaymentService)
    {
        _transactionRepository = transactionRepository;
        _invoicePaymentService = invoicePaymentService;
    }

    [HttpGet("")]
    public Task<List<Transaction>> GetTransactionsAsync()
    {
        return _transactionRepository
            .AsQueryable()
            .OrderBy(t => t.Date)
            .ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Transaction>> GetTransactionAsync(Guid id)
    {
        Transaction? transaction = await _transactionRepository
            .AsQueryable()
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transaction == null)
        {
            return NotFound();
        }

        return transaction;
    }

    [HttpPost("")]
    public async Task<ActionResult<Transaction>> PostTransactionAsync(
        IInvoicePaymentService.CreateInvoicePaymentRequest request)
    {
        try
        {
            return await _invoicePaymentService.CreateAsync(request);
        }
        catch (DomainException e)
        {
            return BadRequest(new
            {
                Message = e.Message,
            });
        }
    }
}
