using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Entities;

public abstract class Transaction : IAggregateRoot
{
    internal Transaction() { }

    public Guid Id { get; init; }

    public BrlAmount Value { get; init; }
}
