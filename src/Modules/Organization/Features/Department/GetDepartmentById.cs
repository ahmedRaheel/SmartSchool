using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.Department;

public static class GetDepartmentById
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
    string? MetadataJson,
    Guid? CampusId,
    string? CampusCode,
    string? CampusName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public interface IGetDepartmentByIdQuery
    {
        Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class GetDepartmentByIdQuery(
        IDbConnectionFactory connectionFactory) : IGetDepartmentByIdQuery
    {
        public async Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                const string sql = """
                    SELECT
                        entity.tenant_id AS "TenantId",
                        entity.department_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.telephone AS "Telephone",
                        entity.email AS "Email",
                        entity.metadata_json AS "MetadataJson",
                        p1.campus_id AS "CampusId",
                        p1.code AS "CampusCode",
                        p1.name AS "CampusName"
                    FROM org.department AS entity
                    LEFT JOIN org.campus AS p1
                        ON p1.campus_id = entity.campus_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.department_id = @Id
                      AND entity.is_active = TRUE;
                    """;

                await using var connection =
                    await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

                return await connection.QuerySingleOrDefaultAsync<Response>(
                    new CommandDefinition(
                        sql,
                        new { TenantId = tenantId, Id = id },
                        cancellationToken: cancellationToken)).ConfigureAwait(false);
            }
    }

    public sealed class Handler(IGetDepartmentByIdQuery query)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(DepartmentEntity))));
            }
            return Result<Response>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "department"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetDepartmentById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        return endpoints;
    }
}
