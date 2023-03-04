using System.Linq.Expressions;
using MusicSchool.Finance.Domain.Entities;

namespace MusicSchool.Finance.Domain.Specifications;

public interface ISpecification<TEntity> where TEntity : IAggregateRoot
{
    Expression<Func<TEntity, bool>> AsPredicate();
}
