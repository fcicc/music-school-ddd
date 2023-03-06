using Microsoft.Extensions.DependencyInjection;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Domain.Services.Internal;

namespace MusicSchool.Finance.Domain.Services;

public class TransactionService : ITransactionService
{
    private readonly IRepository<Transaction> _transactionRepository;
    private readonly Dictionary<Type, Func<ITransactionFactory>> _factoryCreators;

    public TransactionService(
        IRepository<Transaction> transactionRepository,
        IServiceProvider provider)
    {
        _transactionRepository = transactionRepository;
        _factoryCreators = new()
        {
            {
                typeof(ITransactionService.CreateExtraPaymentRequest),
                () => ActivatorUtilities.CreateInstance<ExtraPaymentFactory>(provider)
            },
            {
                typeof(ITransactionService.CreateInvoicePaymentRequest),
                () => ActivatorUtilities.CreateInstance<InvoicePaymentFactory>(provider)
            }
        };
    }

    public async Task<Transaction> CreateAsync(
        ITransactionService.CreateTransactionRequest request)
    {
        if (_factoryCreators.TryGetValue(request.GetType(), out Func<ITransactionFactory>? createFactory))
        {
            ITransactionFactory factory = createFactory();

            Transaction transaction = await factory.CreateAsync(request);
            await _transactionRepository.AddAsync(transaction);

            return transaction;
        }

        throw new DomainException("Unknown transaction type.");
    }
}
