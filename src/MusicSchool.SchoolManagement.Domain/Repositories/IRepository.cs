using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    IQueryable<TEntity> AsQueryable();

    Task AddAsync(TEntity entity);
}
