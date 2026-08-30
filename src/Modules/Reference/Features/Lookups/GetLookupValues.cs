using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetLookupValues
{
    public sealed record Response(long Id, string TypeCode, string Code, string Name, int SortOrder);
    public interface IGetLookupValues { Task<IReadOnlyList<Response>> ExecuteAsync(string typeCode, CancellationToken cancellationToken); }
    internal sealed class GetLookupValuesPersistence(IDbConnectionFactory connectionFactory) : IGetLookupValues
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(string typeCode, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT v.lookup_value_id AS "Id", t.code AS "TypeCode", v.code AS "Code", v.name AS "Name", v.sort_order AS "SortOrder"
                FROM saas.lookup_value v JOIN saas.lookup_type t ON t.lookup_type_id = v.lookup_type_id
                WHERE t.code = @TypeCode AND v.is_active = TRUE ORDER BY v.sort_order, v.name;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql, new { TypeCode = typeCode }, cancellationToken: cancellationToken))).AsList();
        }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/lookups/{typeCode}", async (string typeCode, IGetLookupValues query, CancellationToken cancellationToken) =>
            Results.Ok(await query.ExecuteAsync(typeCode, cancellationToken))).WithTags("Lookups").WithName("GetLookupValues");
    }
}
