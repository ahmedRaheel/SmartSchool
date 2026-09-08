using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Finance.Features.FeeType;

public static class GetFeeTypesByDepartmentId
{
    public sealed record Response(Guid FeeTypeId, Guid DepartmentId, string Code, string Name, string Frequency, string? Description);

    public sealed record Query(Guid TenantId, Guid DepartmentId) : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetFeeTypesByDepartmentIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, Guid departmentId, CancellationToken cancellationToken);
    }

    internal sealed class GetFeeTypesByDepartmentIdQuery(IDbConnectionFactory connectionFactory)
        : IGetFeeTypesByDepartmentIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, Guid departmentId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    fee_type_id AS "FeeTypeId",
                    department_id AS "DepartmentId",
                    code AS "Code",
                    name AS "Name",
                    frequency AS "Frequency",
                    description AS "Description"
                FROM finance.fee_type
                WHERE tenant_id = @TenantId
                  AND department_id = @DepartmentId
                  AND is_active = TRUE
                ORDER BY name;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync<Response>(new CommandDefinition(sql, new { TenantId = tenantId, DepartmentId = departmentId }, cancellationToken: cancellationToken));
            return rows.AsList();
        }
    }

    public sealed class Handler(IGetFeeTypesByDepartmentIdQuery query)
        : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(Query request, CancellationToken cancellationToken)
            => Result<IReadOnlyCollection<Response>>.Success(await query.GetAsync(request.TenantId, request.DepartmentId, cancellationToken));
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/finance/fee-type/by-department/{departmentId:guid}", async (Guid departmentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
            (await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(new Query(tenantId, departmentId), cancellationToken)).ToHttpResult())
            .WithName("GetFeeTypesByDepartmentId")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
