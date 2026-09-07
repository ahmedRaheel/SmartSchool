using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.Timetable;

public static class GetTimetablePage
{
    /// <summary>
    /// Represents the response returned by this TimetableEntity feature.
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
    Guid AcademicYearId,
    string? AcademicYearCode,
    string? AcademicYearName,
    Guid CampusId,
    string? CampusCode,
    string? CampusName,
    Guid? TermId,
    string? TermCode,
    string? TermName);

    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

    public interface IGetTimetablePageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetTimetablePageQuery(
        IDbConnectionFactory connectionFactory) : IGetTimetablePageQuery
    {
        public async Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken)
            {
                const string countSql = """
                    SELECT COUNT(*)
                    FROM academic.timetable AS entity
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE;
                    """;

                const string pageSql = """
                    SELECT
                    entity.tenant_id AS "TenantId",
                    entity.timetable_id AS "Id",
                    entity.code AS "Code",
                    entity.name AS "Name",
                    entity.metadata_json AS "MetadataJson",
                        p1.academic_year_id AS "AcademicYearId",
                        p1.code AS "AcademicYearCode",
                        p1.name AS "AcademicYearName",
                        p2.campus_id AS "CampusId",
                        p2.code AS "CampusCode",
                        p2.name AS "CampusName",
                        p3.term_id AS "TermId",
                        p3.code AS "TermCode",
                        p3.name AS "TermName"
                    FROM academic.timetable AS entity
                    LEFT JOIN academic.academic_year AS p1
                        ON p1.academic_year_id = entity.academic_year_id
                    LEFT JOIN org.campus AS p2
                        ON p2.campus_id = entity.campus_id
                    LEFT JOIN academic.term AS p3
                        ON p3.term_id = entity.term_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE
                    ORDER BY entity.timetable_id
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

    public sealed class Handler(IGetTimetablePageQuery query)
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
                ApiRoutes.EntityCollection("academics", "timetable"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTimetablePage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        return endpoints;
    }
}
