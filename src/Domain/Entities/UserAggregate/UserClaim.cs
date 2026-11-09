namespace Domain.Entities.UserAggregate;

public class UserClaim : IdentityUserClaim<long>
{
    public new long Id { get; set; }
    public virtual User User { get; set; } = null!;
}
