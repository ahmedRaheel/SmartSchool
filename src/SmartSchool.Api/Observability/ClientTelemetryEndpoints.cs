using System.Diagnostics;
using System.Text.Json;
using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Api.Observability;

public sealed record ClientErrorRequest(
    string Message,
    string? Stack,
    string? Url,
    string? Method,
    int? Status,
    string? CorrelationId,
    string? TraceId,
    string? UserAgent,
    DateTimeOffset OccurredAt);

public static class ClientTelemetryEndpoints
{
    public static IEndpointRouteBuilder MapClientTelemetryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/telemetry/client-errors", async (
            ClientErrorRequest request,
            HttpContext context,
            ILoggerFactory factory,
            IDbConnectionFactory connectionFactory,
            CancellationToken cancellationToken) =>
        {
            var logger = factory.CreateLogger("SmartSchool.Client");
            var correlationId = request.CorrelationId ?? context.TraceIdentifier;
            var traceId = request.TraceId ?? Activity.Current?.TraceId.ToString();

            using (logger.BeginScope(new Dictionary<string, object?>
            {
                ["ClientCorrelationId"] = correlationId,
                ["ClientTraceId"] = traceId,
                ["ClientUrl"] = request.Url,
                ["ClientStatus"] = request.Status
            }))
            {
                logger.LogError("Portal error: {Message}. Stack: {Stack}", request.Message, request.Stack);
            }

            const string sql = """
                INSERT INTO observability.application_log
                    (timestamp_utc, level, service, message, exception, trace_id,
                     correlation_id, request_path, properties)
                VALUES
                    (@OccurredAt, 'ERROR', 'SmartSchool.Portal', @Message, @Stack, @TraceId,
                     @CorrelationId, @Url, CAST(@Properties AS jsonb));
                """;

            try
            {
                await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
                await connection.ExecuteAsync(new CommandDefinition(
                    sql,
                    new
                    {
                        request.OccurredAt,
                        request.Message,
                        request.Stack,
                        TraceId = traceId,
                        CorrelationId = correlationId,
                        request.Url,
                        Properties = JsonSerializer.Serialize(new
                        {
                            request.Method,
                            request.Status,
                            request.UserAgent
                        })
                    },
                    cancellationToken: cancellationToken));
            }
            catch (Exception exception)
            {
                // Client telemetry must never break the portal because the log store is unavailable.
                logger.LogWarning(exception, "Could not persist portal telemetry to the database");
            }

            return Results.Accepted(value: new { correlationId, traceId });
        }).AllowAnonymous().WithTags("Telemetry");

        return endpoints;
    }
}
