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
                string? Mobile, string? Email, string? LogoUrl);

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
                SELECT tenant_id AS "TenantId", school_id AS "SchoolId", code AS "Code", name AS "Name", branch_type AS "BranchType", branch_gender_type_id AS "BranchGenderTypeId", academic_system_id AS "AcademicSystemId", address AS "Address", city AS "City", province AS "Province", country AS "Country", phone AS "Phone", fax AS "Fax", mobile AS "Mobile", email AS "Email", logo_url AS "LogoUrl"
                FROM org.campus
                WHERE tenant_id = @TenantId
                  AND campus_id = @Id
                  AND is_active = TRUE;
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
