namespace Domain.Entities.UserAggregate;

public class UserRole : IdentityUserRole<long>
{
    public long Id { get; set; }

    public virtual Role Role { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
