namespace Domain.Entities.UserAggregate.Specifications.RoleSpecs;
public class ByNameSpec(string name) : Specification<Role>
{
    public override Expression<Func<Role, bool>> IsSatisfiedBy()
    {
        return p => string.IsNullOrEmpty(p.Name) && p.Name!.Trim() == name.Trim();
    }
}
