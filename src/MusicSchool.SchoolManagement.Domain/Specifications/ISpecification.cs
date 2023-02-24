using System.Linq.Expressions;
using MusicSchool.SchoolManagement.Domain.Entities;

namespace MusicSchool.SchoolManagement.Domain.Specifications;

public interface ISpecification<TEntity> where TEntity : IAggregateRoot
{
    Expression<Func<TEntity, bool>> AsPredicate();
}
