using Domain.Entities.UserAggregate;
using Infrastructure.Configurations.Markers;

namespace Infrastructure.Configurations.AuthContextRelated;

public class RoleClaimConfig : BaseConfiguration<RoleClaim>, IAuthEntityConfiguration
{
    public override void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        base.Configure(builder);
        builder
             .HasOne(x => x.Role)
             .WithMany(x => x.RoleClaims)
             .HasForeignKey(x => x.RoleId)
             .OnDelete(DeleteBehavior.Cascade)
         ;
    }
}
