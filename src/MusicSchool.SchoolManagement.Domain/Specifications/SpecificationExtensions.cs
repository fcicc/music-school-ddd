using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

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
