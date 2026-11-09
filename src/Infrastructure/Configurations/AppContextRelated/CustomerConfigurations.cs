using Domain.Entities.CustomerAggregate;
using Infrastructure.Configurations.Markers;

namespace Infrastructure.Configurations.AppContextRelated;
public class CustomerConfigurations : ApplicationConfiguration<Customer>, IAppEntityConfiguration
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        base.Configure(builder);
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Code).HasMaxLength(10);
    }
}
