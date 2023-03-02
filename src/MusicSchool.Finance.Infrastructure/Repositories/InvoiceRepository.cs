using Microsoft.EntityFrameworkCore;
using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Infrastructure.DataAccess;

namespace MusicSchool.Finance.Infrastructure.Repositories;

public class InvoiceRepository : Repository<Invoice>
{
    public InvoiceRepository(FinanceContext context)
        : base(context)
    {
    }

    public override IQueryable<Invoice> AsQueryable()
    {
        return base.AsQueryable().Include(i => i.Items);
    }
}
