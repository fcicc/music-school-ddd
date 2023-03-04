using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Domain.Specifications;

public static class SpecificationExtensions
{
    public static bool IsSatisfiedBy<TEntity>(
        this ISpecification<TEntity> specification,
        TEntity entity)
        where TEntity : IAggregateRoot
    {
        Func<TEntity, bool> predicate = specification.AsPredicate().Compile();
        return predicate(entity);
    }
}
