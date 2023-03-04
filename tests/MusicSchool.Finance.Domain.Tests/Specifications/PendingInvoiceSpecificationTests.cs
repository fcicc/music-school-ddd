using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Specifications;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Domain.Tests.Specifications;

public class PendingInvoiceSpecificationTests
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

        List<InvoicePayment> invoicePayments = new();
        if (isPaidInvoice)
        {
            invoicePayments.Add(new()
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
            });
        }

        PendingInvoiceSpecification sut = new(atMonth, invoicePayments.AsQueryable());

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
            true,
        };

        yield return new object[]
        {
            false,
            new DateMonthOnly(2023, 2),
            false,
        };

        yield return new object[]
        {
            false,
            new DateMonthOnly(2023, 3),
            false,
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
