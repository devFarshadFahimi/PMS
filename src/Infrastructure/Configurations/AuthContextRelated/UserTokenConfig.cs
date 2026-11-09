using Domain.Entities.UserAggregate;
using Infrastructure.Configurations.Markers;

namespace Infrastructure.Configurations.AuthContextRelated;

public class UserTokenConfig : BaseConfiguration<UserToken>, IAuthEntityConfiguration
{
    public override void Configure(EntityTypeBuilder<UserToken> builder)
    {
        base.Configure(builder);

        builder.HasKey(x => x.Id);
        builder.Property(p => p.Value).HasMaxLength(short.MaxValue);

        builder
            .HasOne(x => x.User)
             .WithMany(x => x.UserTokens)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade)
         ;
    }
}
