using MockQueryable.Moq;
using Moq;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Domain.Tests.Services;

public class InvoicePaymentServiceTests
{
    private readonly Mock<IRepository<InvoicePayment>> _invoicePaymentRepositoryMock;
    private readonly Mock<IRepository<Invoice>> _invoiceRepositoryMock;

    private readonly InvoicePaymentService _sut;

    public InvoicePaymentServiceTests()
    {
        _invoicePaymentRepositoryMock = new();
        _invoiceRepositoryMock = new();

        _sut = new(
            _invoicePaymentRepositoryMock.Object,
            _invoiceRepositoryMock.Object
        );
    }

    [Fact]
    public async Task PayInvoiceAsync_WithValidInput_GeneratesInvoicePayment()
    {
        IInvoicePaymentService.PayInvoiceRequest request = new()
        {
            InvoiceId = Guid.NewGuid(),
            Value = 200,
        };

        _invoiceRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Invoice>
            {
                new()
                {
                    Id = request.InvoiceId,
                },
            }.BuildMock());

        _invoicePaymentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<InvoicePayment>().BuildMock());

        InvoicePayment invoicePayment = await _sut.PayInvoiceAsync(request);

        Assert.NotEqual(Guid.Empty, invoicePayment.Id);
        Assert.Equal(request.Value, invoicePayment.Value);
        Assert.Equal(request.InvoiceId, invoicePayment.InvoiceId);

        _invoicePaymentRepositoryMock.Verify(r => r.AddAsync(invoicePayment), Times.Once);
    }

    [Fact]
    public async Task PayInvoiceAsync_WithInvalidValue_ThrowsDomainException()
    {
        IInvoicePaymentService.PayInvoiceRequest request = new()
        {
            InvoiceId = Guid.NewGuid(),
            Value = -1,
        };

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.PayInvoiceAsync(request)
        );

        Assert.Equal("Value cannot be less than zero.", exception.Message);

        _invoicePaymentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<InvoicePayment>()), Times.Never);
    }

    [Fact]
    public async Task PayInvoiceAsync_WithNonExistingInvoice_ThrowsDomainException()
    {
        IInvoicePaymentService.PayInvoiceRequest request = new()
        {
            InvoiceId = Guid.NewGuid(),
            Value = 200,
        };

        _invoiceRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Invoice>().BuildMock());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.PayInvoiceAsync(request)
        );

        Assert.Equal("Invoice not found.", exception.Message);

        _invoicePaymentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<InvoicePayment>()), Times.Never);
    }

    [Fact]
    public async Task PayInvoiceAsync_WithPaidInvoice_ThrowsDomainException()
    {
        IInvoicePaymentService.PayInvoiceRequest request = new()
        {
            InvoiceId = Guid.NewGuid(),
            Value = 200,
        };

        _invoiceRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Invoice>
            {
                new()
                {
                    Id = request.InvoiceId,
                },
            }.BuildMock());

        _invoicePaymentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<InvoicePayment>
            {
                new()
                {
                    InvoiceId = request.InvoiceId,
                },
            }.BuildMock());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.PayInvoiceAsync(request)
        );

        Assert.Equal("This invoice is already paid.", exception.Message);

        _invoicePaymentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<InvoicePayment>()), Times.Never);
    }
}
