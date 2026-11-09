using Application;
using Application.Common;
using Application.Common.Services;
using BusinessMakerFramework.Application.Service;
using BusinessMakerFramework.Infrastructure.EventStore.Extensions;
using Infrastructure.Interceptors;
using Infrastructure.Options.Extensions;
using Infrastructure.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjections
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddApplicationServices();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        _ = services.AddDbContext<AuthDbContext>((p, options) =>
        {
            _ = options.UseNpgsql(connectionString);
            _ = options.AddInterceptors(p.GetRequiredService<EventStoreInterceptor>());
        });

        _ = services.AddDbContext<ApplicationDbContext>((p, options) =>
        {
            _ = options.UseNpgsql(connectionString);
            _ = options.AddInterceptors(
                p.GetRequiredService<EventStoreInterceptor>(),
                p.GetRequiredService<AuditStoreInterceptor>());
        });

        _ = services.AddSingleton<EventStoreInterceptor>();
        _ = services.AddSingleton<AuditStoreInterceptor>();

        _ = services.AddDataProtection()
            .PersistKeysToDbContext<AuthDbContext>()
            .SetDefaultKeyLifetime(TimeSpan.FromDays(365));

        _ = services.AddScoped<IAuthDbContext, AuthDbContext>();
        _ = services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        _ = services.AddScoped<IAuthService, AuthService>();

        services.AddEventStoreContext(configuration.GetConnectionString("EventStore")!);

        services.AddOptionsSetups();
        _ = services.AddDateTimeService();
    }
}
