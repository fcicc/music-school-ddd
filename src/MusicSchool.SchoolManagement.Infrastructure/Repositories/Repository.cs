using Microsoft.EntityFrameworkCore;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Domain.Specifications;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;

namespace MusicSchool.SchoolManagement.Infrastructure.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    private readonly SchoolManagementContext _context;

    public Repository(SchoolManagementContext context)
    {
        _context = context;
    }

    public Task<TEntity?> FindOneAsync(Guid id)
    {
        return AsQueryable()
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();
    }

    public Task<List<TEntity>> FindAsync(params ISpecification<TEntity>[] specifications)
    {
        IQueryable<TEntity> queryable = AsQueryable();

        foreach (ISpecification<TEntity> specification in specifications)
        {
            queryable = queryable.Where(specification.AsPredicate());
        }

        return queryable.ToListAsync();
    }

    public Task AddAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        return _context.SaveChangesAsync();
    }

    public virtual IQueryable<TEntity> AsQueryable()
    {
        return _context.Set<TEntity>();
    }
}
