using Microsoft.EntityFrameworkCore;
using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Infrastructure.DataAccess;
using MusicSchool.SchoolManagement.Repositories;

namespace MusicSchool.SchoolManagement.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IAggregateRoot
{
    private readonly SchoolManagementContext _context;

    public Repository(SchoolManagementContext context)
    {
        _context = context;
    }

    public Task AddAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        return _context.SaveChangesAsync();
    }

    public Task<TEntity?> FindOneAsync(Guid id)
    {
        return _context.Set<TEntity>()
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();
    }
}
