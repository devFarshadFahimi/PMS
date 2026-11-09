using Domain.Entities.UserAggregate;
using Infrastructure.Configurations.Markers;

namespace Infrastructure.Configurations.AuthContextRelated;

public class UserLoginConfig : BaseConfiguration<UserLogin>, IAuthEntityConfiguration
{
    public override void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        base.Configure(builder);
        builder
             .HasOne(x => x.User)
             .WithMany(x => x.UserLogins)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade)
         ;
    }
}
