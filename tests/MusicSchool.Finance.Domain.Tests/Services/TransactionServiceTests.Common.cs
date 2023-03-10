using Moq;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services;

namespace MusicSchool.Finance.Domain.Tests.Services;

public class TransactionServiceTests_Common
{
    private readonly Mock<IRepository<Transaction>> _transactionRepositoryMock;

    private readonly TransactionService _sut;

    public TransactionServiceTests_Common()
    {
        _transactionRepositoryMock = new();

        _sut = new(
            _transactionRepositoryMock.Object,
            new Mock<IServiceProvider>().Object
        );
    }

    [Fact]
    public async Task CreateAsync_WithUnknownTransactionType_ThrowsDomainException()
    {
        ITransactionService.CreateTransactionRequest request =
            new Mock<ITransactionService.CreateTransactionRequest>().Object;

        DomainException exception = await Assert.ThrowsAsync<DomainException>(
            () => _sut.CreateAsync(request)
        );

        Assert.Equal("Unknown transaction type.", exception.Message);

        _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Never);
    }
}
