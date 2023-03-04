using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Repositories;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;

namespace MusicSchool.SchoolManagement.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    private readonly SchoolManagementContext _context;

    public Repository(SchoolManagementContext context)
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
}
