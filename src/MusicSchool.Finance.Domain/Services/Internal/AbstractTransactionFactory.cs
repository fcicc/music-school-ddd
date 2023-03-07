using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Domain.Services.Internal;

internal abstract class AbstractTransactionFactory<TRequest, TTransaction>
    : ITransactionFactory
    where TRequest : ITransactionService.CreateTransactionRequest
    where TTransaction : Transaction
{
    public async Task<Transaction> CreateAsync(
        ITransactionService.CreateTransactionRequest request)
    {
        return await CreateAsync((TRequest)request);
    }

    protected abstract Task<TTransaction> CreateAsync(TRequest request);
}
