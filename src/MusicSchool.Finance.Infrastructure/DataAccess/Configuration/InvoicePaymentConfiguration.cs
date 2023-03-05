using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Infrastructure.DataAccess.Configuration;

internal class InvoicePaymentConfiguration : IEntityTypeConfiguration<InvoicePayment>
{
    public void Configure(EntityTypeBuilder<InvoicePayment> builder)
    {
        builder.HasBaseType<Transaction>();

        builder.Property(p => p.InvoiceId).HasColumnName("invoice_id");
        builder.HasOne(typeof(Invoice))
            .WithMany()
            .HasForeignKey(nameof(InvoicePayment.InvoiceId));
    }
}
