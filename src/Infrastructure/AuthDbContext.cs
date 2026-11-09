using Application.Common;
using BusinessMakerFramework.Domain.Core.ValueObjects;
using Domain.Entities.UserAggregate;
using Infrastructure.Configurations.Markers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure;

public class AuthDbContext(DbContextOptions<AuthDbContext> options)
    : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options),
        IAuthDbContext,
        IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    public Task SaveChangeAsync(CancellationToken cancellationToken = default) => SaveChangesAsync(cancellationToken);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly, p => p.IsAssignableTo(typeof(IAuthEntityConfiguration)));
        //var databaseSeeder = new DatabaseSeeder(passwordHasher);

        //modelBuilder.Entity<User>().HasData(databaseSeeder.Users);
        //modelBuilder.Entity<Role>().HasData(databaseSeeder.Roles);
        //modelBuilder.Entity<UserRole>().HasData(databaseSeeder.UserRoles);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>().HaveMaxLength(50);
        configurationBuilder.IgnoreAny<BusinessId>();
    }
}
