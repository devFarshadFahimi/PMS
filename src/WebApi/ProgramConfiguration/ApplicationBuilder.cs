namespace WebApi.ProgramConfiguration;

public static class ApplicationBuilder
{
    public static void UseWebApiConfiguration(this WebApplication app)
    {
        app.UseFrameworkExceptionHandler();
        _ = app.MapFrameworkMapPrometheusScrapingEndpoint();
        app.UseFrameworkSerilogRequestLogging();

        _ = app.UseBuildingBlockSwagger(Environments.Development, docTitle: "وب سرویس");
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

        app.UseHealthCheckEndpoints();

        _ = app.MapControllers();
        app.Run();
    }
}
