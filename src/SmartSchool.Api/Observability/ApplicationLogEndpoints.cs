using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Api.Observability;

/// <summary>Read-only API used by the operations/telemetry UI.</summary>
public static class ApplicationLogEndpoints
{
    public sealed record ApplicationLogRow(
        long Id,
        DateTimeOffset TimestampUtc,
        string Level,
        string? Service,
        string Message,
        string? Exception,
        string? TraceId,
        string? CorrelationId,
        string? RequestPath,
        string? Properties);

    public static IEndpointRouteBuilder MapApplicationLogEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/telemetry/logs")
            .RequireAuthorization()
            .WithTags("Telemetry");

        group.MapGet("/", GetPageAsync);
        group.MapGet("/{id:long}", GetByIdAsync);
        return endpoints;
    }

    private static async Task<IResult> GetPageAsync(
        int page,
        int pageSize,
        string? level,
        string? search,
        IDbConnectionFactory connectionFactory,
        CancellationToken cancellationToken)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize <= 0 ? 50 : pageSize, 1, 200);

        const string countSql = """
            SELECT COUNT(*)
            FROM observability.application_log
            WHERE (@Level IS NULL OR level = @Level)
              AND (@Search IS NULL OR message ILIKE '%' || @Search || '%'
                   OR exception ILIKE '%' || @Search || '%'
                   OR correlation_id ILIKE '%' || @Search || '%'
                   OR trace_id ILIKE '%' || @Search || '%');
            """;

        const string pageSql = """
            SELECT id AS "Id", timestamp_utc AS "TimestampUtc", level AS "Level",
                   service AS "Service", message AS "Message", exception AS "Exception",
                   trace_id AS "TraceId", correlation_id AS "CorrelationId",
                   request_path AS "RequestPath", properties::text AS "Properties"
            FROM observability.application_log
            WHERE (@Level IS NULL OR level = @Level)
              AND (@Search IS NULL OR message ILIKE '%' || @Search || '%'
                   OR exception ILIKE '%' || @Search || '%'
                   OR correlation_id ILIKE '%' || @Search || '%'
                   OR trace_id ILIKE '%' || @Search || '%')
            ORDER BY timestamp_utc DESC
            LIMIT @PageSize OFFSET @Offset;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var parameters = new
        {
            Level = string.IsNullOrWhiteSpace(level) ? null : level.ToUpperInvariant(),
            Search = string.IsNullOrWhiteSpace(search) ? null : search.Trim(),
            PageSize = pageSize,
            Offset = (page - 1) * pageSize
        };

        var total = await connection.ExecuteScalarAsync<long>(
            new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
        var items = (await connection.QueryAsync<ApplicationLogRow>(
            new CommandDefinition(pageSql, parameters, cancellationToken: cancellationToken))).AsList();

        return Results.Ok(new { page, pageSize, total, items });
    }

    private static async Task<IResult> GetByIdAsync(
        long id,
        IDbConnectionFactory connectionFactory,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT id AS "Id", timestamp_utc AS "TimestampUtc", level AS "Level",
                   service AS "Service", message AS "Message", exception AS "Exception",
                   trace_id AS "TraceId", correlation_id AS "CorrelationId",
                   request_path AS "RequestPath", properties::text AS "Properties"
            FROM observability.application_log WHERE id = @Id;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var row = await connection.QuerySingleOrDefaultAsync<ApplicationLogRow>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
        return row is null ? Results.NotFound() : Results.Ok(row);
    }
}
