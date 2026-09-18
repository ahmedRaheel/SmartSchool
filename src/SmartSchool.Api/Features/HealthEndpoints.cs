using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Api.Features;

public static class HealthEndpoints
{
    public static IEndpointRouteBuilder MapSmartSchoolHealth(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/health/live", () => Results.Ok(new { status = "Healthy" })).AllowAnonymous();
        endpoints.MapGet("/health/ready", ReadyAsync).AllowAnonymous();
        return endpoints;
    }

    private static async Task<IResult> ReadyAsync(
        IDbConnectionFactory connectionFactory,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            await connection.ExecuteScalarAsync<int>(
                new CommandDefinition("SELECT 1", cancellationToken: cancellationToken));

            return Results.Ok(new { status = "Ready", database = "Healthy" });
        }
        catch (Exception exception)
        {
            loggerFactory.CreateLogger("SmartSchool.Health")
                .LogError(exception, "Readiness check failed.");

            return Results.Json(
                new { status = "NotReady", database = "Unhealthy" },
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }
    }
}
