using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.Department;

public static class GetDepartmentPage
{
    /// <summary>
    /// Represents the response returned by this DepartmentEntity feature.
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
    string? Telephone,
    string? Email,
    Guid? CampusId,
    Guid? HeadOfDepartmentEmployeeId,
    string? MetadataJson,
    string? CampusCode,
    string? CampusName);

    public sealed record Query(
        Guid TenantId,
        Guid? BranchId = null,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<Response>>>;

    public interface IGetDepartmentPageQuery
    {
        Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken);

    }

    internal sealed class GetDepartmentPageQuery(
        IDbConnectionFactory connectionFactory) : IGetDepartmentPageQuery
    {
        public async Task<PagedResult<Response>> GetPageAsync(
                Guid tenantId,
                int page,
                int pageSize,
                CancellationToken cancellationToken)
            {
                const string countSql = """
                    SELECT COUNT(*)
                    FROM org.department AS entity
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE;
                    """;

                const string pageSql = """
                    SELECT
                    entity.tenant_id AS "TenantId",
                    entity.department_id AS "Id",
                    entity.code AS "Code",
                    entity.name AS "Name",
                    entity.telephone AS "Telephone",
                    entity.email AS "Email",
                    entity.campus_id AS "CampusId",
                    entity.head_of_department_employee_id AS "HeadOfDepartmentEmployeeId",
                    entity.metadata_json AS "MetadataJson",
                        p1.code AS "CampusCode",
                        p1.name AS "CampusName"
                    FROM org.department AS entity
                    LEFT JOIN org.campus AS p1
                        ON p1.campus_id = entity.campus_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.is_active = TRUE
                    ORDER BY entity.department_id
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

    public sealed class Handler(IGetDepartmentPageQuery query)
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
            var pageItems = request.BranchId.HasValue
                ? page.Items.Where(x => x.CampusId == request.BranchId.Value)
                : page.Items;
            var response = new PagedResult<Response>(
                pageItems.ToArray(),
                page.Page,
                page.PageSize,
                page.TotalCount);
            return Result<PagedResult<Response>>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "department"),
                async (Guid tenantId, Guid? branchId, int? page, int? pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, branchId, page ?? 1, pageSize ?? 25);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetDepartmentPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        return endpoints;
    }
}
