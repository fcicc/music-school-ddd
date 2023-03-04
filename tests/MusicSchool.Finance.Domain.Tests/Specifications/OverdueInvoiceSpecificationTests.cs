using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Specifications;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Tests.Specifications;

public class OverdueInvoiceSpecificationTests
{
    [Theory]
    [MemberData(nameof(GenerateTestData))]
    public void IsSatisfiedBy_WithSpecifiedInfo_ReturnsExpectedResult(
        bool isPaidInvoice,
        DateMonthOnly atMonth,
        bool expectedResult)
    {
        Invoice invoice = new()
        {
            Id = Guid.NewGuid(),
            Month = new DateMonthOnly(2023, 1),
        };

        List<Transaction> transactions = new();
        if (isPaidInvoice)
        {
            transactions.Add(new InvoicePayment
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
            });
        }

        OverdueInvoiceSpecification sut = new(atMonth, transactions.AsQueryable());

        Assert.Equal(expectedResult, sut.IsSatisfiedBy(invoice));
    }

    public static IEnumerable<object[]> GenerateTestData()
    {
        yield return new object[]
        {
            false,
            new DateMonthOnly(2022, 12),
            false,
        };

        yield return new object[]
        {
            false,
            new DateMonthOnly(2023, 1),
            false,
        };

        yield return new object[]
        {
            false,
            new DateMonthOnly(2023, 2),
            true,
        };

        yield return new object[]
        {
            false,
            new DateMonthOnly(2023, 3),
            true,
        };

        yield return new object[]
        {
            true,
            new DateMonthOnly(2022, 12),
            false,
        };

        yield return new object[]
        {
            true,
            new DateMonthOnly(2023, 1),
            false,
        };

        yield return new object[]
        {
            true,
            new DateMonthOnly(2023, 2),
            false,
        };

        yield return new object[]
        {
            true,
            new DateMonthOnly(2023, 3),
            false,
        };
    }
}
