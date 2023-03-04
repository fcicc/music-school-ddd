using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Infrastructure.DataAccess.Configuration;

namespace MusicSchool.Finance.Infrastructure.DataAccess;

public class FinanceContext : DbContext
{
    public FinanceContext(DbContextOptions<FinanceContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new InvoiceConfiguration())
            .ApplyConfiguration(new InvoiceItemConfiguration())
            .ApplyConfiguration(new InvoicePaymentConfiguration())
            .ApplyConfiguration(new TransactionConfiguration());
    }
}
