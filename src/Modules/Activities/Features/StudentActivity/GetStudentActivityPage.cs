using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.StudentActivity;

public static class GetStudentActivityPage
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid ActivityId,
        string ActivityCode,
        string ActivityName,
        Guid StudentId,
        string StudentNumber,
        string StudentName,
        string? RoleName,
        DateOnly JoinedAt,
        DateOnly? LeftAt);

    public sealed record Query(Guid? TenantId, Guid? ActivityId, Guid? StudentId, int Page = 1, int PageSize = 50)
        : IRequest<Result<PagedResult<Response>>>;

    public interface IGetStudentActivityPage
    {
        Task<PagedResult<Response>> ExecuteAsync(Guid tenantId, Guid? branchId, Guid? activityId, Guid? studentId, int page, int pageSize, CancellationToken cancellationToken);
    }

    internal sealed class GetStudentActivityPageQuery(IDbConnectionFactory connectionFactory) : IGetStudentActivityPage
    {
        public async Task<PagedResult<Response>> ExecuteAsync(Guid tenantId, Guid? branchId, Guid? activityId, Guid? studentId, int page, int pageSize, CancellationToken cancellationToken)
        {
            const string countSql = """
                SELECT COUNT(sa.student_activity_id)
                FROM activity.student_activity sa
                JOIN student.student s ON s.student_id=sa.student_id AND s.tenant_id=sa.tenant_id
                WHERE sa.tenant_id=@TenantId
                  AND sa.is_active=TRUE
                  AND (@BranchId IS NULL OR s.branch_id=@BranchId)
                  AND (@ActivityId IS NULL OR sa.activity_id=@ActivityId)
                  AND (@StudentId IS NULL OR sa.student_id=@StudentId);
                """;

            const string pageSql = """
                SELECT
                    sa.tenant_id AS "TenantId",
                    sa.student_activity_id AS "Id",
                    sa.activity_id AS "ActivityId",
                    a.code AS "ActivityCode",
                    a.name AS "ActivityName",
                    sa.student_id AS "StudentId",
                    s.student_number AS "StudentNumber",
                    trim(concat_ws(' ', s.first_name, s.last_name)) AS "StudentName",
                    sa.role_name AS "RoleName",
                    sa.joined_at AS "JoinedAt",
                    sa.left_at AS "LeftAt"
                FROM activity.student_activity sa
                JOIN activity.activity a ON a.activity_id=sa.activity_id AND a.tenant_id=sa.tenant_id
                JOIN student.student s ON s.student_id=sa.student_id AND s.tenant_id=sa.tenant_id
                WHERE sa.tenant_id=@TenantId
                  AND sa.is_active=TRUE
                  AND (@BranchId IS NULL OR s.branch_id=@BranchId)
                  AND (@ActivityId IS NULL OR sa.activity_id=@ActivityId)
                  AND (@StudentId IS NULL OR sa.student_id=@StudentId)
                ORDER BY a.activity_date DESC, s.first_name, s.last_name
                LIMIT @PageSize OFFSET @Offset;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var parameters = new
            {
                TenantId = tenantId,
                BranchId = branchId,
                ActivityId = activityId,
                StudentId = studentId,
                PageSize = pageSize,
                Offset = (page - 1) * pageSize
            };
            var total = await connection.ExecuteScalarAsync<long>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
            var items = (await connection.QueryAsync<Response>(new CommandDefinition(pageSql, parameters, cancellationToken: cancellationToken))).AsList();
            return new PagedResult<Response>(items, page, pageSize, total);
        }
    }

    public sealed class Handler(IGetStudentActivityPage query, ITenantScope tenantScope, ICurrentUser currentUser)
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
            var studentId = currentUser.IsInRole(SmartSchoolRoles.Student) ? currentUser.StudentId : request.StudentId;

            var result = await query.ExecuteAsync(tenantId.Value, branchId, request.ActivityId, studentId, page.NormalizedPage, page.NormalizedPageSize, cancellationToken);
            return Result<PagedResult<Response>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student-activity"),
                async (Guid? tenantId, Guid? activityId, Guid? studentId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Query, Result<PagedResult<Response>>>(new Query(tenantId, activityId, studentId, page, pageSize), cancellationToken)).ToHttpResult())
            .WithName("GetStudentActivityPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
