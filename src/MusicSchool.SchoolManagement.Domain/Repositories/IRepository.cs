using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Repositories;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    Task<TEntity?> FindOneAsync(Guid id);

    Task AddAsync(TEntity entity);
}
