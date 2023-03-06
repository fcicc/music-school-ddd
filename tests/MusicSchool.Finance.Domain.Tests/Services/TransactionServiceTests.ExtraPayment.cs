using Moq;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Domain.Tests.Services;

public class TransactionServiceTests_ExtraPayment
{
    private readonly Mock<IRepository<Transaction>> _transactionRepositoryMock;

    private readonly TransactionService _sut;

    public TransactionServiceTests_ExtraPayment()
    {
        _transactionRepositoryMock = new();

        _sut = new(
            _transactionRepositoryMock.Object,
            new Mock<IServiceProvider>().Object
        );
    }

    [Fact]
    public async Task CreateAsync_WithExtraPaymentValidInput_GeneratesExtraPayment()
    {
        ITransactionService.CreateExtraPaymentRequest request = new()
        {
            Description = "foo",
            Date = new DateOnly(2023, 3, 4),
            Value = 200,
        };

        Transaction transaction = await _sut.CreateAsync(request);

        ExtraPayment extraPayment = Assert.IsType<ExtraPayment>(transaction);
        Assert.NotEqual(Guid.Empty, extraPayment.Id);
        Assert.Equal(request.Date, extraPayment.Date);
        Assert.Equal(request.Value, extraPayment.Value);
        Assert.Equal(request.Description, extraPayment.Description);

        _transactionRepositoryMock.Verify(r => r.AddAsync(transaction), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithExtraPaymentInvalidValue_ThrowsDomainException()
    {
        ITransactionService.CreateExtraPaymentRequest request = new()
        {
            Description = "foo",
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
    public async Task CreateAsync_WithExtraPaymentEmptyDescription_ThrowsDomainException()
    {
        ITransactionService.CreateExtraPaymentRequest request = new()
        {
            Description = "",
            Date = new DateOnly(2023, 3, 4),
            Value = 200,
        };

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("Description should not be empty.", exception.Message);

        _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
    }
}