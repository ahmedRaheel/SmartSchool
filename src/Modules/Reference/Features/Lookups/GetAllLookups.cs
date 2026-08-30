using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetAllLookups
{
    public sealed record ValueResponse(long Id, string TypeCode, string Code, string Name, int SortOrder);
    public sealed record Response(string Code, string Name, IReadOnlyList<ValueResponse> Values);
    private sealed record Row(string TypeCode, string TypeName, long? Id, string? Code, string? Name, int? SortOrder);
    public interface IGetAllLookups { Task<IReadOnlyList<Response>> ExecuteAsync(CancellationToken cancellationToken); }
    internal sealed class GetAllLookupsPersistence(IDbConnectionFactory connectionFactory) : IGetAllLookups
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT t.code AS "TypeCode", t.name AS "TypeName", v.lookup_value_id AS "Id", v.code AS "Code", v.name AS "Name", v.sort_order AS "SortOrder"
                FROM saas.lookup_type t LEFT JOIN saas.lookup_value v ON v.lookup_type_id = t.lookup_type_id AND v.is_active = TRUE
                ORDER BY t.name, v.sort_order, v.name;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = (await connection.QueryAsync<Row>(new CommandDefinition(sql, cancellationToken: cancellationToken))).AsList();
            return rows.GroupBy(x => new { x.TypeCode, x.TypeName }).Select(g => new Response(g.Key.TypeCode, g.Key.TypeName,
                g.Where(x => x.Id.HasValue).Select(x => new ValueResponse(x.Id!.Value, x.TypeCode, x.Code!, x.Name!, x.SortOrder ?? 0)).ToList())).ToList();
        }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/lookups", async (IGetAllLookups query, CancellationToken cancellationToken) => Results.Ok(await query.ExecuteAsync(cancellationToken)))
            .WithTags("Lookups").WithName("GetAllLookups");
    }
}
