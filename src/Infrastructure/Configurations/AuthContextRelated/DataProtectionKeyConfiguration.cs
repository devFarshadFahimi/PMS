using Infrastructure.Configurations.Markers;

namespace Infrastructure.Configurations.AuthContextRelated;

public class DataProtectionKeyConfiguration : BaseConfiguration<DataProtectionKey>, IAuthEntityConfiguration
{
    public override void Configure(EntityTypeBuilder<DataProtectionKey> builder)
    {
        base.Configure(builder);
        builder.Property(p => p.FriendlyName).HasMaxLength(short.MaxValue);
        builder.Property(p => p.Xml).HasMaxLength(short.MaxValue);
    }
}
