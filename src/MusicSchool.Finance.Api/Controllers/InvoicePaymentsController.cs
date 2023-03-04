using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Api.Controllers;

[ApiController]
[Route("/invoicePayments")]
public class InvoicePaymentsController : ControllerBase
{
    private readonly IRepository<InvoicePayment> _invoicePaymentRepository;
    private readonly IInvoicePaymentService _invoicePaymentService;

    public InvoicePaymentsController(
        IRepository<InvoicePayment> invoicePaymentRepository,
        IInvoicePaymentService invoicePaymentService)
    {
        _invoicePaymentRepository = invoicePaymentRepository;
        _invoicePaymentService = invoicePaymentService;
    }

    [HttpGet("")]
    public Task<List<InvoicePayment>> GetInvoicePaymentsAsync()
    {
        return _invoicePaymentRepository
            .AsQueryable()
            .OrderBy(p => p.Date)
            .ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InvoicePayment>> GetInvoicePaymentAsync(Guid id)
    {
        InvoicePayment? invoicePayment = await _invoicePaymentRepository
            .AsQueryable()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (invoicePayment == null)
        {
            return NotFound();
        }

        return invoicePayment;
    }

    [HttpPost("")]
    public async Task<ActionResult<InvoicePayment>> PostInvoicePaymentAsync(
        IInvoicePaymentService.PayInvoiceRequest request)
    {
        try
        {
            return await _invoicePaymentService.PayInvoiceAsync(request);
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
