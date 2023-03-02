using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.ValueObjects;

namespace MusicSchool.Finance.Infrastructure.DataAccess.Configuration;

internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("invoices");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("id");

        builder.Property(i => i.StudentId).HasColumnName("student_id");

        builder.Property(i => i.StudentName).HasColumnName("student_name").IsRequired();

        builder.Property(i => i.Month).HasColumnName("month")
            .HasColumnType("char(7)")
            .HasConversion(
                m => m.ToString(),
                v => DateMonthOnly.Parse(v)
            );

        builder.Property(i => i.TotalValue).HasColumnName("total_value")
            .HasConversion(a => a.Value, v => new BrlAmount(v));
    }
}
