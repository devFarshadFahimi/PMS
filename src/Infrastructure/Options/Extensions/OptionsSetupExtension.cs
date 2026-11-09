using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Options.Extensions;

public static class OptionsSetupExtension
{
    public static void AddOptionsSetups(this IServiceCollection services)
    {
        services.AddOptions<AuthTokenOption>("AuthSettings");
    }

    private static void AddOptions<TOption>(this IServiceCollection services, string configurationSectionName)
        where TOption : class
    {
        _ = services.AddOptions<TOption>()
          .BindConfiguration(configurationSectionName)
          .ValidateDataAnnotations()
          .ValidateOnStart();
    }
}
