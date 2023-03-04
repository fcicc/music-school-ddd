using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Repositories;
using MusicSchool.Finance.Infrastructure.DataAccess;

namespace MusicSchool.Finance.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    private readonly FinanceContext _context;

    public Repository(FinanceContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> AsQueryable()
    {
        return _context.Set<TEntity>();
    }

    public Task AddAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        return _context.SaveChangesAsync();
    }

    public Task AddRangeAsync(params TEntity[] entities)
    {
        _context.Set<TEntity>().AddRange(entities);
        return _context.SaveChangesAsync();
    }
}
