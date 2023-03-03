using Microsoft.AspNetCore.Mvc;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Api.Controllers;

[ApiController]
[Route("/invoices")]
public class InvoicesController : ControllerBase
{
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly IInvoiceService _invoiceService;

    public InvoicesController(
        IRepository<Invoice> invoiceRepository,
        IInvoiceService invoiceService)
    {
        _invoiceRepository = invoiceRepository;
        _invoiceService = invoiceService;
    }

    [HttpGet("")]
    public Task<List<Invoice>> GetInvoicesAsync()
    {
        return _invoiceRepository.FindAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Invoice>> GetInvoiceAsync(Guid id)
    {
        Invoice? invoice = await _invoiceRepository.FindOneAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        return invoice;
    }

    [HttpPost("generate")]
    public async Task<ActionResult<List<Invoice>>> PostGenerateInvoicesAsync(
        IInvoiceService.GenerateInvoicesForStudentRequest request)
    {
        try
        {
            return await _invoiceService.GenerateInvoicesForStudentAsync(request);
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
