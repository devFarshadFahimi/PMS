namespace Domain.Entities.UserAggregate.Specifications.UserSpecs;
public class ByPhoneNumberSpec : Specification<User>
{
    private readonly string _phoneNumber;

    public ByPhoneNumberSpec(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }

    public override Expression<Func<User, bool>> IsSatisfiedBy()
    {
        return p => p.PhoneNumber == _phoneNumber;
    }
}
