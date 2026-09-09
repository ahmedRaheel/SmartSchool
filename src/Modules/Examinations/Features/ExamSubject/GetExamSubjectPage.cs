using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.Application.Identity;

namespace SmartSchool.Modules.Examinations.Features.ExamSubject;

public static class GetExamSubjectPage
{
    /// <summary>
    /// Represents the response returned by this ExamSubjectEntity feature.
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
    Guid CourseOfferingId,
    string? CourseOfferingCode,
    string? CourseOfferingName,
    Guid ExamId,
    string? ExamCode,
    string? ExamName,
    Guid? RoomId,
    string? RoomCode,
    string? RoomName);

    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

    public interface IGetExamSubjectPageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetExamSubjectPageQuery(
        IDbConnectionFactory connectionFactory, 
        ICurrentUser currentUser) : IGetExamSubjectPageQuery
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
                    FROM exam.exam_subject AS entity                                        
                    	 join exam.exam  ex on ex.exam_id = entity.exam_id
                    WHERE tenant_id = @TenantId
                    AND (@BranchId IS NULL OR ex.branch_id = @BranchId)
                      AND is_active = TRUE;
                    """;

                const string pageSql = """
                    SELECT
                    tenant_id AS "TenantId",
                    entity.exam_subject_id AS "Id",
                    entity.code AS "Code",
                    entity.name AS "Name",
                    entity.metadata_json AS "MetadataJson",
                        p1.course_offering_id AS "CourseOfferingId",
                        p1.code AS "CourseOfferingCode",
                        p1.name AS "CourseOfferingName",
                        p2.exam_id AS "ExamId",
                        p2.code AS "ExamCode",
                        p2.name AS "ExamName",
                        p3.room_id AS "RoomId",
                        p3.code AS "RoomCode",
                        p3.name AS "RoomName"
                    FROM exam.exam_subject AS entity
                    LEFT JOIN academic.course_offering AS p1
                        ON p1.course_offering_id = entity.course_offering_id
                    JOIN exam.exam AS p2
                        ON p2.exam_id = entity.exam_id
                    LEFT JOIN org.room AS p3
                        ON p3.room_id = entity.room_id
                    WHERE tenant_id = @TenantId
                      AND is_active = TRUE
                      AND (@BranchId IS NULL OR p2.branch_id = @BranchId)
                    ORDER BY entity.exam_subject_id
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
                        cancellationToken: cancellationToken)).ConfigureAwait(false);

                var items = (await connection.QueryAsync<Response>(
                    new CommandDefinition(
                        pageSql,
                        parameters,
                        cancellationToken: cancellationToken)).ConfigureAwait(false))
                    .AsList();

                return new PagedResult<Response>(
                    items,
                    page,
                    pageSize,
                    totalCount);
            }
    }

    public sealed class Handler(IGetExamSubjectPageQuery query)
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
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "exam-subject"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetExamSubjectPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
