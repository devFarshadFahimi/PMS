using Domain.Entities.CustomerAggregate;
using Domain.Entities.UserAggregate;

namespace Application.Common;

public interface IAuthDbContext
{
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleClaim> RoleClaims { get; set; }

    Task SaveChangeAsync(CancellationToken cancellationToken = default);
}

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; set; }
    Task SaveChangeAsync(CancellationToken cancellationToken = default);
}
