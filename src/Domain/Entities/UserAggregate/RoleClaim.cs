namespace Domain.Entities.UserAggregate;

public class RoleClaim : IdentityRoleClaim<long>
{
    public new long Id { get; set; }

    public virtual Role Role { get; set; } = null!;
}
