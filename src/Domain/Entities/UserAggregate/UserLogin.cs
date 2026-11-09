namespace Domain.Entities.UserAggregate;

public class UserLogin : IdentityUserLogin<long>
{
    public long Id { get; set; }

    public virtual User User { get; set; } = null!;
}
