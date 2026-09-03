using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Reference.Features.Lookups;

public static class GetLookupValues
{
    public sealed record Response(long Id, string TypeCode, string Code, string Name, int SortOrder, bool IsTenantScoped, bool CanManage);
    public sealed record Request(string TypeCode, Guid? TenantId = null) : IRequest<IReadOnlyList<Response>>;
    public interface IGetLookupValues { Task<IReadOnlyList<Response>> ExecuteAsync(string typeCode, Guid? tenantId, CancellationToken cancellationToken); }
    internal sealed class GetLookupValuesPersistence(IDbConnectionFactory connectionFactory) : IGetLookupValues
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(string typeCode, Guid? tenantId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT v.lookup_value_id AS "Id", t.code AS "TypeCode", v.code AS "Code", v.name AS "Name",
                       v.sort_order AS "SortOrder", t.is_tenant_scoped AS "IsTenantScoped",
                       (t.is_tenant_scoped AND v.tenant_id = @TenantId) AS "CanManage"
                FROM saas.lookup_value v
                JOIN saas.lookup_type t ON t.lookup_type_id = v.lookup_type_id
                WHERE t.code = @TypeCode AND v.is_active = TRUE
                  AND ((t.is_tenant_scoped = FALSE AND v.tenant_id IS NULL)
                    OR (t.is_tenant_scoped = TRUE AND v.tenant_id = @TenantId))
                ORDER BY v.sort_order, v.name;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql,
                new { TypeCode = typeCode.Trim().ToUpperInvariant(), TenantId = tenantId }, cancellationToken: cancellationToken))).AsList();
        }
    }
    public sealed class Handler(IGetLookupValues query, ITenantScope tenantScope) : IRequestHandler<Request, IReadOnlyList<Response>>
    {
        public Task<IReadOnlyList<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
            => query.ExecuteAsync(request.TypeCode, tenantScope.Resolve(request.TenantId), cancellationToken);
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints
        .MapGet("/api/lookups/{typeCode}", async (string typeCode, Guid? tenantId, IMediator mediator, CancellationToken ct) =>
            Results.Ok(await mediator.SendAsync<Request, IReadOnlyList<Response>>(new(typeCode, tenantId), ct)))
        .WithTags("Lookups").WithName("GetLookupValues").RequireAuthorization();
}
