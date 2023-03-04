using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Domain.Specifications;

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
