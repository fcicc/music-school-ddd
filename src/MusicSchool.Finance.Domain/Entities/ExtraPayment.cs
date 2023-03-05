namespace MusicSchool.Finance.Domain.Entities;

public class ExtraPayment : Transaction
{
    internal ExtraPayment() { }

    public string Description { get; init; } = "";
}
