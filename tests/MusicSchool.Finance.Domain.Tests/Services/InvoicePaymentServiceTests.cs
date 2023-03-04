using MockQueryable.Moq;
using Moq;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Domain.Tests.Services;

public class InvoicePaymentServiceTests
{
    private readonly Mock<IRepository<Transaction>> _transactionRepositoryMock;
    private readonly Mock<IRepository<Invoice>> _invoiceRepositoryMock;

    private readonly InvoicePaymentService _sut;

    public InvoicePaymentServiceTests()
    {
        _transactionRepositoryMock = new();
        _invoiceRepositoryMock = new();

        _sut = new(
            _transactionRepositoryMock.Object,
            _invoiceRepositoryMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_WithValidInput_GeneratesInvoicePayment()
    {
        IInvoicePaymentService.CreateInvoicePaymentRequest request = new()
        {
            InvoiceId = Guid.NewGuid(),
            Date = new DateOnly(2023, 3, 4),
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

        _transactionRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Transaction>().BuildMock());

        InvoicePayment invoicePayment = await _sut.CreateAsync(request);

        Assert.NotEqual(Guid.Empty, invoicePayment.Id);
        Assert.Equal(request.Date, invoicePayment.Date);
        Assert.Equal(request.Value, invoicePayment.Value);
        Assert.Equal(request.InvoiceId, invoicePayment.InvoiceId);

        _transactionRepositoryMock.Verify(r => r.AddAsync(invoicePayment), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidValue_ThrowsDomainException()
    {
        IInvoicePaymentService.CreateInvoicePaymentRequest request = new()
        {
            InvoiceId = Guid.NewGuid(),
            Date = new DateOnly(2023, 3, 4),
            Value = -1,
        };

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("Value cannot be less than zero.", exception.Message);

        _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithNonExistingInvoice_ThrowsDomainException()
    {
        IInvoicePaymentService.CreateInvoicePaymentRequest request = new()
        {
            InvoiceId = Guid.NewGuid(),
            Date = new DateOnly(2023, 3, 4),
            Value = 200,
        };

        _invoiceRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Invoice>().BuildMock());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("Invoice not found.", exception.Message);

        _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithPaidInvoice_ThrowsDomainException()
    {
        IInvoicePaymentService.CreateInvoicePaymentRequest request = new()
        {
            InvoiceId = Guid.NewGuid(),
            Date = new DateOnly(2023, 3, 4),
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

        _transactionRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Transaction>
            {
                new InvoicePayment()
                {
                    InvoiceId = request.InvoiceId,
                },
            }.BuildMock());

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("This invoice is already paid.", exception.Message);

        _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
    }
}
