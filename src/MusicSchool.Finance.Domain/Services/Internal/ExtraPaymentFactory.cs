using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Exceptions;

namespace MusicSchool.Finance.Domain.Services.Internal;

internal class ExtraPaymentFactory
    : AbstractTransactionFactory<
        ITransactionService.CreateExtraPaymentRequest,
        ExtraPayment>
{
    protected override Task<ExtraPayment> CreateAsync(
        ITransactionService.CreateExtraPaymentRequest request)
    {
        if (request.Value < 0)
        {
            throw new DomainException("Value cannot be less than zero.");
        }

        if (string.IsNullOrEmpty(request.Description))
        {
            throw new DomainException("Description should not be empty.");
        }

        return Task.FromResult(new ExtraPayment
        {
            Id = Guid.NewGuid(),
            Date = request.Date,
            Value = request.Value,
            Description = request.Description,
        });
    }
}