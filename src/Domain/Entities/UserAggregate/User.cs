using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities.UserAggregate;

public class User : IdentityUser<long>
{
    public bool IsSystem { get; set; }

    [ForeignKey("UserId")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    [ForeignKey("UserId")]
    public virtual ICollection<UserClaim> UserClaims { get; set; } = new HashSet<UserClaim>();
    [ForeignKey("UserId")]
    public virtual ICollection<UserLogin> UserLogins { get; set; } = new HashSet<UserLogin>();
    [ForeignKey("UserId")]
    public virtual ICollection<UserToken> UserTokens { get; set; } = new HashSet<UserToken>();
}
