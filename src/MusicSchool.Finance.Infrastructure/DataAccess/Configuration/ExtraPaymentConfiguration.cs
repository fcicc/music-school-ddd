using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Infrastructure.DataAccess.Configuration;

internal class ExtraPaymentConfiguration : IEntityTypeConfiguration<ExtraPayment>
{
    public void Configure(EntityTypeBuilder<ExtraPayment> builder)
    {
        builder.HasBaseType<Transaction>();

        builder.Property(p => p.Description).HasColumnName("description").IsRequired();
    }
}
