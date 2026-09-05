using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Reference.Features.DataAccess.Lookup;

public sealed class LookupQuery(IDbConnectionFactory connectionFactory) : ILookupQuery
{
    public async Task<IReadOnlyList<LookupTypeResponse>> GetTypesAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT lookup_type_id AS "Id", code AS "Code", name AS "Name"
            FROM saas.lookup_type
            ORDER BY name;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<LookupTypeResponse>(
            new CommandDefinition(sql, cancellationToken: cancellationToken))).AsList();
    }

    public async Task<IReadOnlyList<LookupValueResponse>> GetValuesAsync(string typeCode, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT v.lookup_value_id AS "Id", t.code AS "TypeCode",
                   v.code AS "Code", v.name AS "Name", v.sort_order AS "SortOrder"
            FROM saas.lookup_value v
            JOIN saas.lookup_type t ON t.lookup_type_id=v.lookup_type_id
            WHERE t.code=@TypeCode AND v.is_active=TRUE
            ORDER BY v.sort_order, v.name;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return (await connection.QueryAsync<LookupValueResponse>(
            new CommandDefinition(sql, new { TypeCode=typeCode }, cancellationToken:cancellationToken))).AsList();
    }

    public async Task<IReadOnlyList<LookupGroupResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT t.code AS "TypeCode", t.name AS "TypeName",
                   v.lookup_value_id AS "Id", v.code AS "Code", v.name AS "Name",
                   v.sort_order AS "SortOrder"
            FROM saas.lookup_type t
            LEFT JOIN saas.lookup_value v
              ON v.lookup_type_id=t.lookup_type_id AND v.is_active=TRUE
            ORDER BY t.name, v.sort_order, v.name;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows=(await connection.QueryAsync<LookupRow>(
            new CommandDefinition(sql,cancellationToken:cancellationToken))).AsList();

        return rows.GroupBy(x => new { x.TypeCode, x.TypeName })
            .Select(g => new LookupGroupResponse(
                g.Key.TypeCode,
                g.Key.TypeName,
                g.Where(x => x.Id.HasValue)
                 .Select(x => new LookupValueResponse(x.Id!.Value,x.TypeCode,x.Code!,x.Name!,x.SortOrder ?? 0))
                 .ToList()))
            .ToList();
    }

    private sealed record LookupRow(string TypeCode,string TypeName,long? Id,string? Code,string? Name,int? SortOrder);
}
