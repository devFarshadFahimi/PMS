using Domain.Contracts;

namespace Domain.Entities.UserAggregate.Specifications.RoleSpecs;

public class ByRoleIdListSpec<TEntity> : Specification<TEntity>
    where TEntity : IHasRole
{
    private readonly List<long> _ids;

    public ByRoleIdListSpec(List<long> ids)
    {
        _ids = ids;
    }

    public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
    {
        return p => _ids.Contains(p.RoleId);
    }
}
