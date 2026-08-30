using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetLookupTypes
{
    public sealed record Response(long Id, string Code, string Name);

    public interface IGetLookupTypes
    {
        Task<IReadOnlyList<Response>> ExecuteAsync(CancellationToken cancellationToken);
    }

    internal sealed class GetLookupTypesPersistence(IDbConnectionFactory connectionFactory) : IGetLookupTypes
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT lookup_type_id AS "Id", code AS "Code", name AS "Name"
                FROM saas.lookup_type
                ORDER BY name;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql, cancellationToken: cancellationToken))).AsList();
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/lookups/types", async (IGetLookupTypes query, CancellationToken cancellationToken) =>
            Results.Ok(await query.ExecuteAsync(cancellationToken)))
            .WithTags("Lookups").WithName("GetLookupTypes");
    }
}
