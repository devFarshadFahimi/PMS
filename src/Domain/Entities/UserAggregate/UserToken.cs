namespace Domain.Entities.UserAggregate;

public class UserToken : IdentityUserToken<long>
{
    public long Id { get; set; }
    public DateTime ValidTo { get; set; }
    public virtual User User { get; set; } = null!;
}
