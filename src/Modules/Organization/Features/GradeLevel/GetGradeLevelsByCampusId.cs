using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.GradeLevel;

public static class GetGradeLevelsByCampusId
{
    public sealed record Response(Guid Id, string Code, string Name, Guid CampusId);
    public sealed record Query(Guid TenantId, Guid CampusId) : IRequest<Result<IReadOnlyCollection<Response>>>;
    public interface IGetGradeLevelsByCampusIdQuery { Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, Guid campusId, CancellationToken cancellationToken); }
    internal sealed class GetGradeLevelsByCampusIdQuery(IDbConnectionFactory connectionFactory) : IGetGradeLevelsByCampusIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, Guid campusId, CancellationToken cancellationToken)
        {
            const string sql = """SELECT grade_level_id AS "Id", code AS "Code", name AS "Name", campus_id AS "CampusId" FROM academic.grade_level WHERE tenant_id=@TenantId AND campus_id=@CampusId AND is_active=TRUE ORDER BY sort_order, name;""";
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            return (await connection.QueryAsync<Response>(new CommandDefinition(sql, new { TenantId=tenantId, CampusId=campusId }, cancellationToken:cancellationToken))).AsList();
        }
    }
    public sealed class Handler(IGetGradeLevelsByCampusIdQuery query) : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(Query request, CancellationToken cancellationToken) => Result<IReadOnlyCollection<Response>>.Success(await query.GetAsync(request.TenantId, request.CampusId, cancellationToken));
    }
    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/academics/campuses/{campusId:guid}/grade-levels", async (Guid campusId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) => (await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(new Query(tenantId,campusId),cancellationToken)).ToHttpResult())
            .WithName("GetGradeLevelsByCampusId").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        return endpoints;
    }
}
