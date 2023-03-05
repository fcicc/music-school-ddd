using MockQueryable.Moq;
using Moq;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Domain.Tests.Services;

public class TransactionServiceTests
{
    private readonly Mock<IRepository<Transaction>> _transactionRepositoryMock;
    private readonly Mock<IRepository<Invoice>> _invoiceRepositoryMock;

    private readonly TransactionService _sut;

    public TransactionServiceTests()
    {
        _transactionRepositoryMock = new();
        _invoiceRepositoryMock = new();

        _sut = new(
            _transactionRepositoryMock.Object,
            _invoiceRepositoryMock.Object
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

        _transactionRepositoryMock
            .Setup(r => r.AsQueryable())
            .Returns(new List<Transaction>().BuildMock());

        Transaction transaction = await _sut.CreateAsync(request);

        Assert.NotEqual(Guid.Empty, transaction.Id);
        Assert.Equal(request.Date, transaction.Date);
        Assert.Equal(request.Value, transaction.Value);

        InvoicePayment invoicePayment = Assert.IsType<InvoicePayment>(transaction);
        Assert.Equal(request.InvoiceId, invoicePayment.InvoiceId);

        _transactionRepositoryMock.Verify(r => r.AddAsync(transaction), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithNonExistingInvoice_ThrowsDomainException()
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
    public async Task CreateAsync_WithPaidInvoice_ThrowsDomainException()
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

    [Fact]
    public async Task CreateAsync_WithInvalidValue_ThrowsDomainException()
    {
        CreateAnyTransactionRequest request = new()
        {
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
    public async Task CreateAsync_WithUnknownTransactionType_ThrowsDomainException()
    {
        CreateAnyTransactionRequest request = new()
        {
            Date = new DateOnly(2023, 3, 4),
            Value = 200,
        };

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("Unknown transaction type.", exception.Message);

        _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
    }

    private class CreateAnyTransactionRequest : ITransactionService.CreateTransactionRequest { }
}
