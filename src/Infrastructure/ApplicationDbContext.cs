using System.Reflection;
using Application.Common;
using BusinessMakerFramework.Infrastructure.SqlCommand;
using Domain.Entities.CustomerAggregate;
using Infrastructure.Configurations.Markers;

namespace Infrastructure;

public class ApplicationDbContext : BaseCommandDbContext, IApplicationDbContext, IAsyncDisposable
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    public Task SaveChangeAsync(CancellationToken cancellationToken = default) => SaveChangesAsync(cancellationToken);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), p => p.IsAssignableTo(typeof(IAppEntityConfiguration)));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        _ = configurationBuilder.Properties<string>().HaveMaxLength(50);
    }
}
