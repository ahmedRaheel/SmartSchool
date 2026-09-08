using Dapper;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Enums;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Campus;

public static class GetCampusById
{
    /// <summary>
    /// Represents the response returned by this CampusEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
        Guid TenantId, Guid SchoolId, string Code, string Name, BranchType BranchType, Guid BranchGenderTypeId, Guid? AcademicSystemId,
                string? Address, string? City, string? Province, string? Country, string? Phone, string? Fax,
                string? Mobile, string? Email, string? LogoUrl,
    string? SchoolCode,
    string? SchoolName,
    string? AcademicSystemCode,
    string? AcademicSystemName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;    public interface IGetCampusByIdQuery
    {
        Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken);
    }



    internal sealed class GetCampusByIdQuery(IDbConnectionFactory connectionFactory) : IGetCampusByIdQuery
    {
        public async Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT entity.tenant_id AS "TenantId", entity.school_id AS "SchoolId", entity.code AS "Code", entity.name AS "Name", entity.branch_type AS "BranchType", entity.branch_gender_type_id AS "BranchGenderTypeId", entity.academic_system_id AS "AcademicSystemId", entity.address AS "Address", entity.city AS "City", entity.province AS "Province", entity.country AS "Country", entity.phone AS "Phone", entity.fax AS "Fax", entity.mobile AS "Mobile", entity.email AS "Email", entity.logo_url AS "LogoUrl",
                        p1.code AS "SchoolCode",
                        p1.name AS "SchoolName",
                        p2.code AS "AcademicSystemCode",
                        p2.name AS "AcademicSystemName"
                FROM org.campus AS entity
                    LEFT JOIN org.school AS p1
                        ON p1.school_id = entity.school_id
                    LEFT JOIN academic.academic_system AS p2
                        ON p2.academic_system_id = entity.academic_system_id
                WHERE entity.tenant_id = @TenantId
                  AND entity.campus_id = @Id
                  AND entity.is_active = TRUE;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var response = await connection.QuerySingleOrDefaultAsync<Response>(
                new CommandDefinition(sql, new { request.TenantId, request.Id }, cancellationToken: cancellationToken));

            if (response is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Response))));
            }

            return Result<Response>.Success(response);
        }
    }

    public sealed class Handler(IGetCampusByIdQuery query)
        : IRequestHandler<Query, Result<Response>>
    {
        public Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            return query.ExecuteAsync(request, cancellationToken);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "campus"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetCampusById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        return endpoints;
    }
}
