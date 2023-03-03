using MusicSchool.Finance.Domain.Entities;
using MusicSchool.Finance.Domain.Specifications;

namespace MusicSchool.Finance.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    Task<TEntity?> FindOneAsync(Guid id);

    Task<List<TEntity>> FindAsync(params ISpecification<TEntity>[] specifications);

    Task AddAsync(TEntity entity);

    Task AddRangeAsync(params TEntity[] entities);

    IQueryable<TEntity> AsQueryable();
}
