using Domain.Entities.UserAggregate;
using Infrastructure.Configurations.Markers;

namespace Infrastructure.Configurations.AuthContextRelated;

public class RoleConfig : BaseConfiguration<Role>, IAuthEntityConfiguration
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);
    }
}
