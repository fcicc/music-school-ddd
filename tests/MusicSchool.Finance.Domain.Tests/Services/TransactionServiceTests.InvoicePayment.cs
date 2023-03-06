using MockQueryable.Moq;
using Moq;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Domain.Tests.Services;

public class TransactionServiceTests_InvoicePayment
{
    private readonly Mock<IRepository<Transaction>> _transactionRepositoryMock;
    private readonly Mock<IRepository<Invoice>> _invoiceRepositoryMock;
    private readonly Mock<IRepository<InvoicePayment>> _invoicePaymentRepositoryMock;

    private readonly TransactionService _sut;

    public TransactionServiceTests_InvoicePayment()
    {
        _transactionRepositoryMock = new();
        _invoiceRepositoryMock = new();
        _invoicePaymentRepositoryMock = new();

        Mock<IServiceProvider> providerMock = new();

        providerMock
            .Setup(p => p.GetService(typeof(IRepository<Invoice>)))
            .Returns(_invoiceRepositoryMock.Object);

        providerMock
            .Setup(p => p.GetService(typeof(IRepository<InvoicePayment>)))
            .Returns(_invoicePaymentRepositoryMock.Object);

        _sut = new(
            _transactionRepositoryMock.Object,
            providerMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_WithInvoicePaymentValidInput_GeneratesInvoicePayment()
    {
        ITransactionService.CreateInvoicePaymentRequest request = new()
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

        _invoicePaymentRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<InvoicePayment>().BuildMock());

        Transaction transaction = await _sut.CreateAsync(request);

        InvoicePayment invoicePayment = Assert.IsType<InvoicePayment>(transaction);
        Assert.NotEqual(Guid.Empty, invoicePayment.Id);
        Assert.Equal(request.Date, invoicePayment.Date);
        Assert.Equal(request.Value, invoicePayment.Value);
        Assert.Equal(request.InvoiceId, invoicePayment.InvoiceId);

        _transactionRepositoryMock.Verify(r => r.AddAsync(transaction), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithInvoicePaymentInvalidValue_ThrowsDomainException()
    {
        ITransactionService.CreateInvoicePaymentRequest request = new()
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
    public async Task CreateAsync_WithInvoicePaymentNonExistingInvoice_ThrowsDomainException()
    {
        ITransactionService.CreateInvoicePaymentRequest request = new()
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
    public async Task CreateAsync_WithInvoicePaymentPaidInvoice_ThrowsDomainException()
    {
        ITransactionService.CreateInvoicePaymentRequest request = new()
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
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("This invoice is already paid.", exception.Message);
    }
}