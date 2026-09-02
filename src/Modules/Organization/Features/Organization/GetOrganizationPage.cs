using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Organization.Enums;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Organization;

public static class GetOrganizationPage
{
    public sealed record Query(int Page = 1, int PageSize = 25)
        : IRequest<Result<PagedResult<Response>>>;

    public sealed record Response(
        Guid TenantId,
        string Code,
        string OrganizationName,
        string FirstName,
        string LastName,
        TenantStatus Status,
        string? ContactName,
        string? ContactEmail,
        string? ContactPhoneNumber,
        int SchoolCount);

    public interface IGetOrganizationPage
    {
        Task<PagedResult<Response>> GetAsync(int page, int pageSize, CancellationToken cancellationToken);
    }

    internal sealed class Persistence(IDbConnectionFactory connectionFactory) : IGetOrganizationPage
    {
        public async Task<PagedResult<Response>> GetAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            const string countSql = "SELECT COUNT(*) FROM saas.tenant WHERE is_active = TRUE;";
            const string pageSql = """
                SELECT
                    t.tenant_id AS "TenantId",
                    t.code AS "Code",
                    t.name AS "OrganizationName",
                    t.first_name AS "FirstName",
                    t.last_name AS "LastName",
                    t.status_code AS "StatusValue",
                    c.contact_name AS "ContactName",
                    c.email AS "ContactEmail",
                    c.phone AS "ContactPhoneNumber",
                    (SELECT COUNT(*)::int FROM org.school s
                     WHERE s.tenant_id = t.tenant_id AND s.is_active = TRUE) AS "SchoolCount"
                FROM saas.tenant t
                LEFT JOIN LATERAL (
                    SELECT * FROM saas.tenant_contact tc
                    WHERE tc.tenant_id = t.tenant_id AND tc.is_active = TRUE
                    ORDER BY tc.is_primary DESC, tc.created_at
                    LIMIT 1
                ) c ON TRUE
                WHERE t.is_active = TRUE
                ORDER BY t.created_at DESC
                LIMIT @PageSize OFFSET @Offset;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var total = await connection.ExecuteScalarAsync<long>(
                new CommandDefinition(countSql, cancellationToken: cancellationToken));
            var rows = (await connection.QueryAsync<Row>(new CommandDefinition(
                pageSql,
                new { PageSize = pageSize, Offset = (page - 1) * pageSize },
                cancellationToken: cancellationToken))).AsList();

            var items = rows.Select(x => new Response(
                x.TenantId, x.Code, x.OrganizationName, x.FirstName, x.LastName,
                (TenantStatus)x.StatusValue, x.ContactName, x.ContactEmail,
                x.ContactPhoneNumber, x.SchoolCount)).ToList();

            return new PagedResult<Response>(items, page, pageSize, total);
        }

        private sealed record Row(
            Guid TenantId,
            string Code,
            string OrganizationName,
            string FirstName,
            string LastName,
            short StatusValue,
            string? ContactName,
            string? ContactEmail,
            string? ContactPhoneNumber,
            int SchoolCount);
    }

    public sealed class Handler(IGetOrganizationPage persistence)
        : IRequestHandler<Query, Result<PagedResult<Response>>>
    {
        public async Task<Result<PagedResult<Response>>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var normalized = new PageRequest(request.Page, request.PageSize);
            return Result<PagedResult<Response>>.Success(
                await persistence.GetAsync(normalized.NormalizedPage, normalized.NormalizedPageSize, cancellationToken));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/tenancy/tenant",
                async (int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Query, Result<PagedResult<Response>>>(
                        new Query(page, pageSize), cancellationToken)).ToHttpResult())
            .WithName("GetOrganizationPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);

        return endpoints;
    }
}
