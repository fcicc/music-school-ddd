using MusicSchool.SchoolManagement.Domain.Entities;
using MusicSchool.SchoolManagement.Domain.Specifications;

namespace MusicSchool.SchoolManagement.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    Task<TEntity?> FindOneAsync(Guid id);

    Task<List<TEntity>> FindAsync();

    Task<List<TEntity>> FindAsync(ISpecification<TEntity> specification);

    Task AddAsync(TEntity entity);
}
