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
        if (request is TRequest concreteRequest)
        {
            return await CreateAsync(concreteRequest);
        }

        throw new InvalidOperationException("Invalid request type.");
    }

    protected abstract Task<TTransaction> CreateAsync(TRequest request);
}
