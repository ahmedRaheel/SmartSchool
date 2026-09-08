using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.AcademicYear;

public static class GetAcademicYearPage
{
    /// <summary>
    /// Represents the response returned by this AcademicYearEntity feature.
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
    Guid CampusId,
    string? CampusCode,
    string? CampusName,
    Guid? SchoolId,
    string? SchoolCode,
    string? SchoolName);

    public sealed record Query(
        Guid TenantId,
        Guid? CampusId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

    public interface IGetAcademicYearPageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                Guid? campusId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetAcademicYearPageQuery(
        IDbConnectionFactory connectionFactory) : IGetAcademicYearPageQuery
    {
        public async Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                Guid? campusId,
                int page,
                int pageSize,
                CancellationToken cancellationToken)
            {
                const string countSql = """
                    SELECT COUNT(*)
                    FROM academic.academic_year AS entity
                    WHERE entity.tenant_id = @TenantId
                      AND (@CampusId IS NULL OR entity.campus_id = @CampusId)
                      AND entity.is_active = TRUE;
                    """;

                const string pageSql = """
                    SELECT
                    entity.tenant_id AS "TenantId",
                    entity.academic_year_id AS "Id",
                    entity.code AS "Code",
                    entity.name AS "Name",
                    entity.metadata_json AS "MetadataJson",
                        p1.campus_id AS "CampusId",
                        p1.code AS "CampusCode",
                        p1.name AS "CampusName",
                        p2.school_id AS "SchoolId",
                        p2.code AS "SchoolCode",
                        p2.name AS "SchoolName"
                    FROM academic.academic_year AS entity
                    LEFT JOIN org.campus AS p1
                        ON p1.campus_id = entity.campus_id
                    LEFT JOIN org.school AS p2
                        ON p2.school_id = entity.school_id
                    WHERE entity.tenant_id = @TenantId
                      AND (@CampusId IS NULL OR entity.campus_id = @CampusId)
                      AND entity.is_active = TRUE
                    ORDER BY entity.start_date DESC, entity.academic_year_id
                    LIMIT @PageSize OFFSET @Offset;
                    """;

                await using var connection =
                    await connectionFactory.OpenConnectionAsync(cancellationToken);

                var parameters = new
                {
                    TenantId = tenantId,
                    CampusId = campusId,
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

    public sealed class Handler(IGetAcademicYearPageQuery query)
        : IRequestHandler<Query, Result<PagedResult<Response>>>
    {
        public async Task<Result<PagedResult<Response>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(request.Page, request.PageSize);
            var page = await query.GetPageAsync(
                request.TenantId,
                request.CampusId,
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
                ApiRoutes.EntityCollection("academics", "academic-year"),
                async (Guid tenantId, Guid? campusId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, campusId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetAcademicYearPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        return endpoints;
    }
}
