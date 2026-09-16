using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Award;

public static class GetAwardPage
{
    public sealed record Response(
        Guid TenantId,
        Guid Id,
        Guid StudentId,
        string StudentNumber,
        string StudentName,
        string AwardTypeCode,
        string Title,
        string? Description,
        DateOnly AwardDate,
        Guid? ApprovedBy,
        string? ApprovedByName,
        Guid? DocumentId);

    public sealed record Query(Guid? TenantId, Guid? StudentId, int Page = 1, int PageSize = 25)
        : IRequest<Result<PagedResult<Response>>>;

    public interface IGetAwardPage
    {
        Task<PagedResult<Response>> ExecuteAsync(
            Guid tenantId,
            Guid? branchId,
            Guid? studentId,
            int page,
            int pageSize,
            CancellationToken cancellationToken);
    }

    internal sealed class GetAwardPageQuery(IDbConnectionFactory connectionFactory) : IGetAwardPage
    {
        public async Task<PagedResult<Response>> ExecuteAsync(
            Guid tenantId,
            Guid? branchId,
            Guid? studentId,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            const string countSql = """
                SELECT COUNT(*)
                FROM activity.student_award a
                JOIN student.student s ON s.student_id=a.student_id AND s.tenant_id=a.tenant_id
                WHERE a.tenant_id=@TenantId
                  AND a.is_active=TRUE
                  AND (@BranchId IS NULL OR s.branch_id=@BranchId)
                  AND (@StudentId IS NULL OR a.student_id=@StudentId);
                """;

            const string pageSql = """
                SELECT
                    a.tenant_id AS "TenantId",
                    a.student_award_id AS "Id",
                    a.student_id AS "StudentId",
                    s.student_number AS "StudentNumber",
                    trim(concat_ws(' ', s.first_name, s.last_name)) AS "StudentName",
                    a.award_type_code AS "AwardTypeCode",
                    a.title AS "Title",
                    a.description AS "Description",
                    a.award_date AS "AwardDate",
                    a.approved_by AS "ApprovedBy",
                    NULLIF(trim(concat_ws(' ', e.first_name, e.last_name)), '') AS "ApprovedByName",
                    a.generated_document_id AS "DocumentId"
                FROM activity.student_award a
                JOIN student.student s ON s.student_id=a.student_id AND s.tenant_id=a.tenant_id
                LEFT JOIN hr.employee e ON e.employee_id=a.approved_by AND e.tenant_id=a.tenant_id
                WHERE a.tenant_id=@TenantId
                  AND a.is_active=TRUE
                  AND (@BranchId IS NULL OR s.branch_id=@BranchId)
                  AND (@StudentId IS NULL OR a.student_id=@StudentId)
                ORDER BY a.award_date DESC, a.title
                LIMIT @PageSize OFFSET @Offset;
                """;

            var parameters = new
            {
                TenantId = tenantId,
                BranchId = branchId,
                StudentId = studentId,
                PageSize = pageSize,
                Offset = (page - 1) * pageSize
            };

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var total = await connection.ExecuteScalarAsync<long>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
            var items = (await connection.QueryAsync<Response>(new CommandDefinition(pageSql, parameters, cancellationToken: cancellationToken))).AsList();
            return new PagedResult<Response>(items, page, pageSize, total);
        }
    }

    public sealed class Handler(IGetAwardPage query, ITenantScope tenantScope, ICurrentUser currentUser)
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
            var branchId = currentUser.IsInRole(SmartSchoolRoles.SuperAdmin) || currentUser.IsInRole(SmartSchoolRoles.SuperOwner) || currentUser.IsInRole(SmartSchoolRoles.Tenant)
                ? null
                : currentUser.BranchId;
            var studentId = currentUser.IsInRole(SmartSchoolRoles.Student)
                ? currentUser.StudentId
                : request.StudentId;

            var result = await query.ExecuteAsync(
                tenantId.Value,
                branchId,
                studentId,
                page.NormalizedPage,
                page.NormalizedPageSize,
                cancellationToken);

            return Result<PagedResult<Response>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "award"),
                async (Guid? tenantId, Guid? studentId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Query, Result<PagedResult<Response>>>(new Query(tenantId, studentId, page, pageSize), cancellationToken)).ToHttpResult())
            .WithName("GetAwardPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
