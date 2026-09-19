using Dapper;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Api.Testing;

/// <summary>
/// Development-only lifecycle endpoints used by the self-contained API smoke-test fixture.
/// These endpoints are never mapped outside Development.
/// </summary>
public static class SmokeTestFixtureEndpoints
{
    public static IEndpointRouteBuilder MapSmokeTestFixtureEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/testing/auth-probe",
                (HttpContext httpContext) =>
                {
                    var user = httpContext.User;
                    return Results.Ok(new
                    {
                        authenticated = user.Identity?.IsAuthenticated == true,
                        authenticationType = user.Identity?.AuthenticationType,
                        subject = user.FindFirst("sub")?.Value,
                        tenantId = user.FindFirst("tenant_id")?.Value,
                        schoolId = user.FindFirst("school_id")?.Value,
                        branchId = user.FindFirst("branch_id")?.Value,
                        roles = user.FindAll("role").Select(claim => claim.Value).ToArray(),
                        scopes = user.FindAll("scope").Select(claim => claim.Value).ToArray()
                    });
                })
            .WithTags("Testing")
            .RequireAuthorization();

        endpoints.MapDelete(
                "/api/testing/fixtures/{tenantId:guid}",
                CleanupTenantAsync)
            .WithTags("Testing")
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);

        return endpoints;
    }

    private static async Task<IResult> CleanupTenantAsync(
        Guid tenantId,
        [FromQuery] string runId,
        HttpContext httpContext,
        IDbConnectionFactory connectionFactory,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(runId)
            || !runId.StartsWith("smoke-", StringComparison.OrdinalIgnoreCase))
        {
            return Results.BadRequest(
                new { message = "A valid smoke-test runId is required." });
        }

        if (!httpContext.Request.Headers.TryGetValue(
                "X-Smoke-Test-Run",
                out var header)
            || !string.Equals(
                header.ToString(),
                runId,
                StringComparison.Ordinal))
        {
            return Results.BadRequest(
                new { message = "Smoke-test run header does not match runId." });
        }

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        await using var transaction =
            await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            // Development normally runs with the PostgreSQL owner account. Disabling
            // RI triggers inside this transaction makes cleanup deterministic even when
            // the smoke run created a graph of rows across multiple module schemas.
            await connection.ExecuteAsync(
                new CommandDefinition(
                    "SET LOCAL session_replication_role = replica;",
                    transaction: transaction,
                    cancellationToken: cancellationToken));

            const string tableSql = """
                SELECT c.table_schema AS "SchemaName",
                       c.table_name AS "TableName"
                FROM information_schema.columns c
                JOIN information_schema.tables t
                  ON t.table_schema = c.table_schema
                 AND t.table_name = c.table_name
                WHERE c.column_name = 'tenant_id'
                  AND t.table_type = 'BASE TABLE'
                  AND c.table_schema NOT IN
                      ('pg_catalog', 'information_schema')
                ORDER BY c.table_schema, c.table_name;
                """;

            var tables = (
                await connection.QueryAsync<TenantTable>(
                    new CommandDefinition(
                        tableSql,
                        transaction: transaction,
                        cancellationToken: cancellationToken)))
                .ToArray();

            var deleted = new List<object>(tables.Length);
            long totalRows = 0;

            foreach (var table in tables)
            {
                var qualified =
                    $"{QuoteIdentifier(table.SchemaName)}.{QuoteIdentifier(table.TableName)}";

                var count = await connection.ExecuteAsync(
                    new CommandDefinition(
                        $"DELETE FROM {qualified} WHERE tenant_id = @TenantId;",
                        new { TenantId = tenantId },
                        transaction,
                        cancellationToken: cancellationToken));

                if (count <= 0)
                {
                    continue;
                }

                totalRows += count;
                deleted.Add(
                    new
                    {
                        schema = table.SchemaName,
                        table = table.TableName,
                        rows = count
                    });
            }

            await transaction.CommitAsync(cancellationToken);

            return Results.Ok(
                new
                {
                    tenantId,
                    runId,
                    deletedRows = totalRows,
                    affectedTables = deleted
                });
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static string QuoteIdentifier(string identifier) =>
        "\"" + identifier.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";

    private sealed record TenantTable(string SchemaName, string TableName);
}
