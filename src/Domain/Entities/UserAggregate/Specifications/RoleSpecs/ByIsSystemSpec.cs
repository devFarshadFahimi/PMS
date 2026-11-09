namespace Domain.Entities.UserAggregate.Specifications.RoleSpecs;

public class ByIsSystemSpec : Specification<Role>
{
    private readonly bool _isSystem;

    public ByIsSystemSpec(bool isSystem = false)
    {
        _isSystem = isSystem;
    }

    public override Expression<Func<Role, bool>> IsSatisfiedBy()
    {
        return p => p.IsSystem == _isSystem;
    }
}
