using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Enums;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Organization;

public static class GetOrganizationById
{
    public sealed record Query(Guid Id) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        string Code,
        string OrganizationName,
        string FirstName,
        string LastName,
        TenantStatus Status,
        string? MetadataJson,
        ContactResponse? PrimaryContact,
        int SchoolCount);

    public sealed record ContactResponse(
        Guid TenantContactId,
        ContactType ContactType,
        string? ContactName,
        string? Email,
        string? Phone,
        string? AddressLine1,
        bool IsPrimary);

    public interface IGetOrganizationById
    {
        Task<Response?> GetAsync(Guid tenantId, CancellationToken cancellationToken);
    }

    internal sealed class Persistence(IDbConnectionFactory connectionFactory) : IGetOrganizationById
    {
        public async Task<Response?> GetAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    t.tenant_id AS "TenantId",
                    t.code AS "Code",
                    t.name AS "OrganizationName",
                    t.first_name AS "FirstName",
                    t.last_name AS "LastName",
                    t.status_code AS "Status",
                    t.metadata_json::text AS "MetadataJson",
                    c.tenant_contact_id AS "TenantContactId",
                    c.contact_type AS "ContactType",
                    c.contact_name AS "ContactName",
                    c.email AS "Email",
                    c.phone AS "Phone",
                    c.address_line1 AS "AddressLine1",
                    c.is_primary AS "IsPrimary",
                    (SELECT COUNT(*)::int FROM org.school s
                     WHERE s.tenant_id = t.tenant_id AND s.is_active = TRUE) AS "SchoolCount"
                FROM saas.tenant t
                LEFT JOIN LATERAL (
                    SELECT * FROM saas.tenant_contact tc
                    WHERE tc.tenant_id = t.tenant_id
                      AND tc.is_active = TRUE
                    ORDER BY tc.is_primary DESC, tc.created_at
                    LIMIT 1
                ) c ON TRUE
                WHERE t.tenant_id = @TenantId
                  AND t.is_active = TRUE;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var row = await connection.QuerySingleOrDefaultAsync<Row>(
                new CommandDefinition(sql, new { TenantId = tenantId }, cancellationToken: cancellationToken));

            if (row is null)
            {
                return null;
            }

            ContactResponse? contact = row.TenantContactId.HasValue
                ? new ContactResponse(
                    row.TenantContactId.Value,
                    (ContactType)row.ContactType.GetValueOrDefault(),
                    row.ContactName,
                    row.Email,
                    row.Phone,
                    row.AddressLine1,
                    row.IsPrimary.GetValueOrDefault())
                : null;

            return new Response(
                row.TenantId,
                row.Code,
                row.OrganizationName,
                row.FirstName,
                row.LastName,
                (TenantStatus)row.Status,
                row.MetadataJson,
                contact,
                row.SchoolCount);
        }

        private sealed record Row(
            Guid TenantId,
            string Code,
            string OrganizationName,
            string FirstName,
            string LastName,
            short Status,
            string? MetadataJson,
            Guid? TenantContactId,
            short? ContactType,
            string? ContactName,
            string? Email,
            string? Phone,
            string? AddressLine1,
            bool? IsPrimary,
            int SchoolCount);
    }

    public sealed class Handler(IGetOrganizationById persistence)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var response = await persistence.GetAsync(request.Id, cancellationToken);
            return response is null
                ? Result<Response>.Failure(Error.NotFound(ErrorMessages.EntityNotFound(nameof(TenantEntity))))
                : Result<Response>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/tenancy/tenant/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Query, Result<Response>>(new Query(id), cancellationToken)).ToHttpResult())
            .WithName("GetOrganizationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);

        return endpoints;
    }
}
