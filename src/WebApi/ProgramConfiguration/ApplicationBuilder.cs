using System.Runtime.InteropServices;
using Scalar.AspNetCore;

namespace WebApi.ProgramConfiguration;

public static class ApplicationBuilder
{
    public static void UseWebApiConfiguration(this WebApplication app)
    {
        app.UseFrameworkExceptionHandler();
        _ = app.MapFrameworkMapPrometheusScrapingEndpoint();
        app.UseFrameworkSerilogRequestLogging();
        _ = app.MapOpenApi();
        _ = app.MapScalarApiReference(p =>
        {
            ScalarAuthenticationOptions scalarAuthenticationOptions = new();
            int num = 1;
            List<string> list = new(num);
            CollectionsMarshal.SetCount(list, num);
            CollectionsMarshal.AsSpan(list)[0] = "Bearer";
            scalarAuthenticationOptions.PreferredSecuritySchemes = list;
            p.Authentication = scalarAuthenticationOptions;
        });

        _ = app.UseHttpsRedirection();
        _ = app.UseCors(p =>
        {
            _ = p.AllowAnyHeader();
            _ = p.AllowAnyMethod();
            _ = p.AllowAnyOrigin();
            _ = p.SetPreflightMaxAge(TimeSpan.FromMinutes(5));
        });

        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        //app.UseHealthCheckEndpoints();

        _ = app.MapControllers();
        app.Run();
    }
}
