using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Infrastructure.DataAccess.Configuration;

internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    private const string TypeProperty = "Type";

    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasDiscriminator<string>(TypeProperty)
            .HasValue<ExtraPayment>("extra_payment")
            .HasValue<InvoicePayment>("invoice_payment");

        builder.Property(TypeProperty).HasColumnName("type")
            .HasColumnType("char(50)");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id");

        builder.Property(t => t.Date).HasColumnName("date")
            .HasColumnType("date")
            .HasConversion(
                d => new DateTime(d.Year, d.Month, d.Day),
                d => new DateOnly(d.Year, d.Month, d.Day)
            );

        builder.Property(t => t.Value).HasColumnName("value")
            .HasConversion(a => a.Value, v => new BrlAmount(v));
    }
}
