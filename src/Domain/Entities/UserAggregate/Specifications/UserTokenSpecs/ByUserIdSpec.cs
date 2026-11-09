using Domain.Contracts;
namespace Domain.Entities.UserAggregate.Specifications.UserTokenSpecs;
public class ByUserIdSpec<TEntity>(long userId) : Specification<TEntity>
    where TEntity : IHasUser
{
    public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
    {
        return p => p.UserId == userId;
    }
}
