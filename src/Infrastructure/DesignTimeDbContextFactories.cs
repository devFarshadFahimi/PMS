using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;

public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args) => new(
        new DbContextOptionsBuilder<AuthDbContext>()
            .UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? "Host=localhost;Port=5432;Database=pms-application;Username=sa;Password=123456aA@;")
            .Options);
}

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args) => new(
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ?? "Host=localhost;Port=5432;Database=pms-application;Username=sa;Password=123456aA@;")
            .Options);
}
