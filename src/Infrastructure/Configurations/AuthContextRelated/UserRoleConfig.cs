using Domain.Entities.UserAggregate;
using Infrastructure.Configurations.Markers;

namespace Infrastructure.Configurations.AuthContextRelated;

public class UserRoleConfig : BaseConfiguration<UserRole>, IAuthEntityConfiguration
{
    public override void Configure(EntityTypeBuilder<UserRole> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(pt => pt.User)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(pt => pt.UserId);

        builder
            .HasOne(pt => pt.Role)
            .WithMany(t => t.UserRoles)
            .HasForeignKey(pt => pt.RoleId);

    }
}
