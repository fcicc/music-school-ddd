using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Domain.Services.Internal;

internal interface ITransactionFactory
{
    Task<Transaction> CreateAsync(
        ITransactionService.CreateTransactionRequest request);
}
