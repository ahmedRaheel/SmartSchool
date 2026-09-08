using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.AcademicYear;

public static class GetAcademicYearsByCampusId
{
    public sealed record Response(Guid Id, string Code, string Name, Guid CampusId);
    public sealed record Query(Guid TenantId, Guid CampusId) : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetAcademicYearsByCampusIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, Guid campusId, CancellationToken cancellationToken);
    }

    internal sealed class GetAcademicYearsByCampusIdQuery(IDbConnectionFactory connectionFactory) : IGetAcademicYearsByCampusIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(Guid tenantId, Guid campusId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT academic_year_id AS "Id", code AS "Code", name AS "Name", campus_id AS "CampusId"
                FROM academic.academic_year
                WHERE tenant_id = @TenantId AND campus_id = @CampusId AND is_active = TRUE
                ORDER BY start_date DESC, name;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var items = await connection.QueryAsync<Response>(new CommandDefinition(sql, new { TenantId = tenantId, CampusId = campusId }, cancellationToken: cancellationToken));
            return items.AsList();
        }
    }

    public sealed class Handler(IGetAcademicYearsByCampusIdQuery query) : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(Query request, CancellationToken cancellationToken) =>
            Result<IReadOnlyCollection<Response>>.Success(await query.GetAsync(request.TenantId, request.CampusId, cancellationToken));
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/academics/campuses/{campusId:guid}/academic-years", async (Guid campusId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
            (await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(new Query(tenantId, campusId), cancellationToken)).ToHttpResult())
            .WithName("GetAcademicYearsByCampusId").WithTags(ModuleConstants.Name).RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        return endpoints;
    }
}
