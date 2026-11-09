using System.Text;
using Domain.Entities.UserAggregate;
using Infrastructure;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Minio;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace WebApi.ProgramConfiguration;

public class ActionSummaryNamingTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is ControllerActionDescriptor action)
        {
            operation.Summary = action.ActionName;
        }

        return Task.CompletedTask;
    }
}

public sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

    public BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
    {
        _authenticationSchemeProvider = authenticationSchemeProvider;
    }

    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await _authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            var bearerScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter your JWT Bearer token"
            };
            document.Components ??= new OpenApiComponents();

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
            {
                ["Bearer"] = bearerScheme
            };

            // 3. Create a global security requirement that references the Bearer scheme
            var securityRequirement = new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            };

            // 4. Apply the requirement to every operation
            foreach (var operation in document.Paths.Values.Where(p => p.Operations != null).SelectMany(p => p.Operations!))
            {
                operation.Value.Security ??= [];
                operation.Value.Security.Add(securityRequirement);
            }

        }
    }
}


public static class ServiceCollections
{
    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructureServices(builder.Configuration);

        _ = builder.Services.AddOpenApi(options =>
        {
            _ = options.AddOperationTransformer<ActionSummaryNamingTransformer>();
            _ = options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        _ = builder.Services.AddControllers();
        _ = builder.Services.AddBuildingBlockSwagger(builder.Configuration, addBearerScheme: true);
        _ = builder.Services.AddHttpClient().AddHttpContextAccessor();
        _ = builder.Services.AddSingleton<IMinioClient>(
             new MinioClient()
                .WithEndpoint("localhost", 9000)
                .WithCredentials("minioadmin", "minioadmin")
                .Build());

        //_ = builder.WebHost.ConfigureKestrel(options =>
        //{
        //    options.Limits.MaxRequestBodySize = 1073741824;
        //    options.ListenLocalhost(5001, listenOptions =>
        //    {
        //        listenOptions.UseHttps("C:\\dev-cert.pfx", "123456aA@");
        //    });
        //});
        _ = builder.Services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = 1073741824);

        builder.Services.AddAuthService(builder.Configuration);

        _ = builder.Services.AddFrameworkHealthCheck()
          .AddDbContextCheck<ApplicationDbContext>("MainContext")
          .AddRedis("localhost:6379");

        _ = builder.Services.AddFrameworkHealthCheckUI("http://localhost:5010/hc")
            .AddPostgreSqlStorage(builder.Configuration.GetConnectionString("HealthCheckConnection")!);

        if (builder.Environment.IsDevelopment())
        {
            builder.AddFrameworkSerilogWithSeqService();
        }
        else
        {
            builder.AddFrameworkSerilogWithLokiService(new Serilog.Sinks.Grafana.Loki.LokiLabel()
            {
                Key = "Bs",
                Value = "Bs"
            });
        }

        builder.AddFrameworkOpenTelemeteryAndJaegerTracing("business-maker-service");

        builder.Services.AddFrameworkExceptionHandler();

        return builder;
    }

    private static void AddAuthService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 3;

            options.User.RequireUniqueEmail = true;
            //options.Tokens.EmailConfirmationTokenProvider = "EmailConfirmationTokenProviderName";
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();


        var authTokenOption = new AuthTokenOption();
        var jwtConfigSection = configuration.GetSection("AuthTokenOptions");
        _ = services.Configure<AuthTokenOption>(jwtConfigSection);

        jwtConfigSection.Bind(authTokenOption);

        _ = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidAudience = authTokenOption.ValidAudience,
                    ValidIssuer = authTokenOption.ValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authTokenOption.SigningKey))
                };
            });
    }
}
