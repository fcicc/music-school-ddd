using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    IQueryable<TEntity> AsQueryable();

    Task AddAsync(TEntity entity);

    Task AddRangeAsync(params TEntity[] entities);
}
