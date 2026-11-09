# PMS

PMS is a .NET 10 Web API for customer, user, and wallet management. It uses ASP.NET Core Identity and JWT authentication, Entity Framework Core with PostgreSQL, and EventStore for domain events and auditing.

## Structure

- **Domain**: entities, value objects, and domain events.
- **Application**: commands, queries, handlers, and service contracts.
- **Infrastructure**: persistence, identity, event storage, and supporting services.
- **WebApi**: HTTP controllers, authentication, OpenAPI, health checks, and telemetry.

## Run locally

Install the .NET 10 SDK and PostgreSQL. Local database settings are in `src/WebApi/appsettings.Development.json`. The application uses `pms-application`; health-check history uses `pms-application-health`. Ensure the configured PostgreSQL account can access these databases.

The development configuration also references EventStore, Redis, MinIO, Seq, Loki, and an OpenTelemetry collector. Supporting container definitions are provided in the repository.

```sh
dotnet restore
dotnet ef database update --project src/Infrastructure --startup-project src/WebApi --context AuthDbContext
dotnet ef database update --project src/Infrastructure --startup-project src/WebApi --context ApplicationDbContext
dotnet run --project src/WebApi --launch-profile http
```

Both database contexts have their own PostgreSQL `Init` migration. Apply both when setting up a fresh database. Use environment variables or user secrets to override connection strings and authentication settings outside local development.
