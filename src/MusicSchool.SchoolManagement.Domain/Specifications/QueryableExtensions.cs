using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> WithSpecification<TEntity>(
        this IQueryable<TEntity> queryable,
        ISpecification<TEntity> specification)
        where TEntity : IAggregateRoot
    {
        return queryable.Where(specification.AsPredicate());
    }
}
