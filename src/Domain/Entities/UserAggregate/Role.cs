using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.UserAggregate;

public class Role : IdentityRole<long>
{
    public Role(string name)
    {
        Name = name;
        NormalizedName = name.ToUpper();
    }

    public Role()
    {
    }

    [ForeignKey("RoleId")]
    public virtual ICollection<RoleClaim> RoleClaims { get; set; } = [];
    [ForeignKey("RoleId")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];

    public bool IsSystem { get; set; }
}
