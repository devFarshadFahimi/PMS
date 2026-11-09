using Domain.Entities.UserAggregate;
using Infrastructure.Configurations.Markers;

namespace Infrastructure.Configurations.AuthContextRelated;

public class UserClaimConfig : BaseConfiguration<UserClaim>, IAuthEntityConfiguration
{
    public override void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        base.Configure(builder);
        builder
             .HasOne(x => x.User)
             .WithMany(x => x.UserClaims)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade)
         ;
    }
}
