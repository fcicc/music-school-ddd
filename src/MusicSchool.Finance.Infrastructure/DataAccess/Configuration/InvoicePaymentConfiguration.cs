using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Infrastructure.DataAccess.Configuration;

internal class InvoicePaymentConfiguration : IEntityTypeConfiguration<InvoicePayment>
{
    public void Configure(EntityTypeBuilder<InvoicePayment> builder)
    {
        builder.ToTable("invoice_payments");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");

        builder.Property(p => p.Value).HasColumnName("value")
            .HasConversion(a => a.Value, v => new BrlAmount(v));

        builder.Property(p => p.InvoiceId).HasColumnName("invoice_id");
        builder.HasOne(typeof(Invoice))
            .WithMany()
            .HasForeignKey(nameof(InvoicePayment.InvoiceId));
    }
}
