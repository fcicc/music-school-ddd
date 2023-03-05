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
    private readonly ITransactionService _transactionService;

    public TransactionsController(
        IRepository<Transaction> transactionRepository,
        ITransactionService transactionService)
    {
        _transactionRepository = transactionRepository;
        _transactionService = transactionService;
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
        ITransactionService.CreateTransactionRequest request)
    {
        try
        {
            return await _transactionService.CreateAsync(request);
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
