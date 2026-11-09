using Domain.Contracts;
namespace Domain.Entities.UserAggregate.Specifications.RoleSpecs;

public class ByRoleIdSpec<TEntity> : Specification<TEntity>
    where TEntity : IHasRole
{
    private readonly long _roleId;

    public ByRoleIdSpec(long roleId)
    {
        _roleId = roleId;
    }

    public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
    {
        return p => p.RoleId == _roleId;
    }
}
