using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Infrastructure.DataAccess.Configuration;

internal class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    private const string InvoiceIdProperty = "InvoiceId";

    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.ToTable("invoice_items");

        builder.HasOne(typeof(Invoice))
            .WithMany(nameof(Invoice.Items))
            .HasForeignKey(InvoiceIdProperty)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasKey(InvoiceIdProperty, nameof(InvoiceItem.EnrollmentId));
        builder.Property(InvoiceIdProperty).HasColumnName("invoice_id");
        builder.Property(i => i.EnrollmentId).HasColumnName("enrollment_id");

        builder.Property(i => i.Value).HasColumnName("value")
            .HasConversion(a => a.Value, v => new BrlAmount(v));
    }
}
