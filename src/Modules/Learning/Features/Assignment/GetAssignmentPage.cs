using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Features.Assignment;

public static class GetAssignmentPage
{
    /// <summary>
    /// Represents the response returned by this AssignmentEntity feature.
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
    Guid? ClassSectionId,
    string? ClassSectionCode,
    string? ClassSectionName,
    Guid CourseOfferingId,
    string? CourseOfferingCode,
    string? CourseOfferingName,
    Guid? TeachingGroupId,
    string? TeachingGroupName);

    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

    public interface IGetAssignmentPageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetAssignmentPageQuery(
        IDbConnectionFactory connectionFactory) : IGetAssignmentPageQuery
    {
        public async Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken)
            {
                const string countSql = """
                    SELECT COUNT(*)
                    FROM lms.academic_assignment AS entity
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE;
                    """;

                const string pageSql = """
                    SELECT
                    entity.tenant_id AS "TenantId",
                    entity.academic_assignment_id AS "Id",
                    entity.code AS "Code",
                    entity.name AS "Name",
                    entity.metadata_json AS "MetadataJson",
                        p1.class_section_id AS "ClassSectionId",
                        p1.code AS "ClassSectionCode",
                        p1.name AS "ClassSectionName",
                        p2.course_offering_id AS "CourseOfferingId",
                        p2.code AS "CourseOfferingCode",
                        p2.name AS "CourseOfferingName",
                        p3.teaching_group_id AS "TeachingGroupId",
                        p3.name AS "TeachingGroupName"
                    FROM lms.academic_assignment AS entity
                    LEFT JOIN academic.class_section AS p1
                        ON p1.class_section_id = entity.class_section_id
                    LEFT JOIN academic.course_offering AS p2
                        ON p2.course_offering_id = entity.course_offering_id
                    LEFT JOIN academic.teaching_group AS p3
                        ON p3.teaching_group_id = entity.teaching_group_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE
                    ORDER BY entity.academic_assignment_id
                    LIMIT @PageSize OFFSET @Offset;
                    """;

                await using var connection =
                    await connectionFactory.OpenConnectionAsync(cancellationToken);

                var parameters = new
                {
                    TenantId = tenantId,
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

    public sealed class Handler(IGetAssignmentPageQuery query)
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
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "assignment"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAssignmentPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
