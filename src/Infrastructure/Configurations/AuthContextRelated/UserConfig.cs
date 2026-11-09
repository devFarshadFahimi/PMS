using Domain.Entities.UserAggregate;
using Infrastructure.Configurations.Markers;

namespace Infrastructure.Configurations.AuthContextRelated;

public class UserConfig : BaseConfiguration<User>, IAuthEntityConfiguration
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.PasswordHash).HasMaxLength(short.MaxValue);
    }
}
