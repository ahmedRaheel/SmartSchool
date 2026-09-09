using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.Application.Identity;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

public static class GetStudentExamResultPage
{
    /// <summary>
    /// Represents the response returned by this StudentExamResultEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson,
    Guid ExamSubjectId,
    string? ExamSubjectCode,
    string? ExamSubjectName);

    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

    public interface IGetStudentExamResultPageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetStudentExamResultPageQuery(
        IDbConnectionFactory connectionFactory, 
        ICurrentUser currentUser) : IGetStudentExamResultPageQuery
    {
        public async Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken)
            {
                var branchId = currentUser.IsInRole(SmartSchoolRoles.Tenant) ? null : currentUser.BranchId;
            const string countSql = """
                    SELECT COUNT(*)
                    FROM exam.student_exam_result AS entity
                          join exam.exam_subject sb on sb.exam_subject_id = entity.exam_subject_id
                    	 join exam.exam  ex on ex.exam_id = sb.exam_id
                    WHERE tenant_id = @TenantId
                     AND (ex.branch_id = @BranchId OR @BranchId IS NULL)
                      AND is_active = TRUE;
                    """;

                const string pageSql = """
                    SELECT
                    tenant_id AS "TenantId",
                    entity.student_exam_result_id AS "Id",
                    entity.code AS "Code",
                    entity.name AS "Name",
                    entity.metadata_json AS "MetadataJson",
                        p1.exam_subject_id AS "ExamSubjectId",
                        p1.code AS "ExamSubjectCode",
                        p1.name AS "ExamSubjectName"
                    FROM exam.student_exam_result AS entity
                      join exam.exam_subject sb on sb.exam_subject_id = entity.exam_subject_id
                    	 join exam.exam  ex on ex.exam_id = sb.exam_id

                    WHERE tenant_id = @TenantId
                      AND is_active = TRUE
                      AND (ex.branch_id = @BranchId OR @BranchId IS NULL)
                    ORDER BY entity.student_exam_result_id
                    LIMIT @PageSize OFFSET @Offset;
                    """;

                await using var connection =
                    await connectionFactory.OpenConnectionAsync(cancellationToken);

                var parameters = new
                {
                    TenantId = tenantId,
                    BranchId = branchId,
                    PageSize = pageSize,
                    Offset = (page - 1) * pageSize
                };

                var totalCount = await connection.ExecuteScalarAsync<long>(
                    new CommandDefinition(
                        countSql,
                        parameters,
                        cancellationToken: cancellationToken));

                var items = (await connection.QueryAsync<Response>(
                    new CommandDefinition(
                        pageSql,
                        parameters,
                        cancellationToken: cancellationToken)))
                    .AsList();

                return new PagedResult<Response>(
                    items,
                    page,
                    pageSize,
                    totalCount);
            }
    }

    public sealed class Handler(IGetStudentExamResultPageQuery query)
        : IRequestHandler<Query, Result<PagedResult<Response>>>
    {
        public async Task<Result<PagedResult<Response>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(request.Page, request.PageSize);
            var page = await query.GetPageAsync(
                request.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);
            var response = new PagedResult<Response>(
                page.Items,
                page.Page,
                page.PageSize,
                page.TotalCount);
            return Result<PagedResult<Response>>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "student-exam-result"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentExamResultPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
