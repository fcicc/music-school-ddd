using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;
using MusicSchool.Finance.Domain.Specifications;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Api.Controllers;

[ApiController]
[Route("/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(
        IRepository<Invoice> invoiceRepository,
        IRepository<Transaction> transactionRepository,
        IInvoiceService invoiceService)
    {
        _invoiceRepository = invoiceRepository;
        _transactionRepository = transactionRepository;
        _invoiceService = invoiceService;
    }

    [HttpGet("")]
    public Task<List<Invoice>> GetInvoicesAsync(
        [FromQuery] InvoiceStatus status = InvoiceStatus.All)
    {
        IQueryable<Invoice> queryable = _invoiceRepository
            .AsQueryable()
            .Include(i => i.Items);

        switch (status)
        {
            case InvoiceStatus.Paid:
                queryable = queryable.WithSpecification(
                    new PaidInvoiceSpecification(
                        DateMonthOnly.Current,
                        _transactionRepository.AsQueryable()
                    )
                );
                break;

            case InvoiceStatus.Pending:
                queryable = queryable.WithSpecification(
                    new PendingInvoiceSpecification(
                        DateMonthOnly.Current,
                        _transactionRepository.AsQueryable()
                    )
                );
                break;

            case InvoiceStatus.Overdue:
                queryable = queryable.WithSpecification(
                    new OverdueInvoiceSpecification(
                        DateMonthOnly.Current,
                        _transactionRepository.AsQueryable()
                    )
                );
                break;
        }

        return queryable
            .OrderBy(i => i.Month).ThenBy(i => i.StudentName)
            .ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Invoice>> GetInvoiceAsync(Guid id)
    {
        Invoice? invoice = await _invoiceRepository
            .AsQueryable()
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
        {
            return NotFound();
        }

        return invoice;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<List<Invoice>>> PostGenerateInvoicesAsync(
        IInvoiceService.GenerateInvoicesRequest request)
    {
        try
        {
            return await _invoiceService.GenerateInvoicesAsync(request);
        }
        catch (DomainException e)
        {
            return BadRequest(new
            {
                Message = e.Message,
            });
        }
    }

    public enum InvoiceStatus
    {
        All = 0,
        Paid = 1,
        Pending = 2,
        Overdue = 3,
    }
}
