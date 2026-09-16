using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Activity;

public static class GetActivityPage
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        string Code,
        string Name,
        string Category,
        DateOnly ActivityDate,
        TimeOnly? StartTime,
        TimeOnly? EndTime,
        string? Venue,
        string? Description,
        int? MaxParticipants,
        string Status,
        Guid? CampusId,
        string? CampusName,
        Guid? CoordinatorEmployeeId,
        string? CoordinatorName,
        int ParticipantCount);

    public sealed record Query(Guid? TenantId, int Page = 1, int PageSize = 25)
        : IRequest<Result<PagedResult<Response>>>;

    public interface IGetActivityPage
    {
        Task<PagedResult<Response>> ExecuteAsync(Guid tenantId, Guid? branchId, int page, int pageSize, CancellationToken cancellationToken);
    }

    internal sealed class GetActivityPageQuery(IDbConnectionFactory connectionFactory) : IGetActivityPage
    {
        public async Task<PagedResult<Response>> ExecuteAsync(Guid tenantId, Guid? branchId, int page, int pageSize, CancellationToken cancellationToken)
        {
            const string countSql = """
                SELECT COUNT(a.activity_id)
                FROM activity.activity a
                WHERE a.tenant_id=@TenantId
                  AND a.is_active=TRUE
                  AND (@BranchId IS NULL OR a.campus_id=@BranchId);
                """;

            const string pageSql = """
                SELECT
                    a.tenant_id AS "TenantId",
                    a.activity_id AS "Id",
                    a.code AS "Code",
                    a.name AS "Name",
                    a.category AS "Category",
                    a.activity_date AS "ActivityDate",
                    a.start_time AS "StartTime",
                    a.end_time AS "EndTime",
                    a.venue AS "Venue",
                    a.description AS "Description",
                    a.max_participants AS "MaxParticipants",
                    a.status AS "Status",
                    a.campus_id AS "CampusId",
                    c.name AS "CampusName",
                    a.coordinator_employee_id AS "CoordinatorEmployeeId",
                    NULLIF(trim(concat_ws(' ', e.first_name, e.last_name)), '') AS "CoordinatorName",
                    COALESCE((SELECT COUNT(*)::int
                              FROM activity.student_activity sa
                              WHERE sa.tenant_id=a.tenant_id
                                AND sa.activity_id=a.activity_id
                                AND sa.is_active=TRUE
                                AND sa.left_at IS NULL), 0) AS "ParticipantCount"
                FROM activity.activity a
                LEFT JOIN org.campus c ON c.campus_id=a.campus_id
                LEFT JOIN hr.employee e ON e.employee_id=a.coordinator_employee_id
                WHERE a.tenant_id=@TenantId
                  AND a.is_active=TRUE
                  AND (@BranchId IS NULL OR a.campus_id=@BranchId)
                ORDER BY a.activity_date DESC, a.name
                LIMIT @PageSize OFFSET @Offset;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var parameters = new
            {
                TenantId = tenantId,
                BranchId = branchId,
                PageSize = pageSize,
                Offset = (page - 1) * pageSize
            };

            var total = await connection.ExecuteScalarAsync<long>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
            var items = (await connection.QueryAsync<Response>(new CommandDefinition(pageSql, parameters, cancellationToken: cancellationToken))).AsList();
            return new PagedResult<Response>(items, page, pageSize, total);
        }
    }

    public sealed class Handler(IGetActivityPage query, ITenantScope tenantScope, ICurrentUser currentUser)
        : IRequestHandler<Query, Result<PagedResult<Response>>>
    {
        public async Task<Result<PagedResult<Response>>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<PagedResult<Response>>.Failure(Error.Validation("Tenant context is required."));
            }

            var page = new PageRequest(request.Page, request.PageSize);
            var branchId = currentUser.IsInRole(SmartSchoolRoles.SuperAdmin) || currentUser.IsInRole(SmartSchoolRoles.Tenant)
                ? null
                : currentUser.BranchId;

            var result = await query.ExecuteAsync(tenantId.Value, branchId, page.NormalizedPage, page.NormalizedPageSize, cancellationToken);
            return Result<PagedResult<Response>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "activity"),
                async (Guid? tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Query, Result<PagedResult<Response>>>(new Query(tenantId, page, pageSize), cancellationToken)).ToHttpResult())
            .WithName("GetActivityPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
