namespace Domain.Entities.UserAggregate.Specifications.UserSpecs;

public class ByEmailSpec : Specification<User>
{
    private readonly string _email;

    public ByEmailSpec(string email)
    {
        _email = email;
    }

    public override Expression<Func<User, bool>> IsSatisfiedBy()
    {
        return p => p.Email == _email;
    }
}
