using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Organization.Features.GradeLevel;

public static class GetGradeLevelById
{
    /// <summary>
    /// Represents the response returned by this GradeLevelEntity feature.
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
    Guid? AcademicSystemId,
    string? AcademicSystemCode,
    string? AcademicSystemName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public interface IGetGradeLevelByIdQuery
    {
        Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class GetGradeLevelByIdQuery(
        IDbConnectionFactory connectionFactory) : IGetGradeLevelByIdQuery
    {
        public async Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                const string sql = """
                    SELECT
                        entity.tenant_id AS "TenantId",
                        entity.grade_level_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.campus_id AS "CampusId",
                        p1.code AS "CampusCode",
                        p1.name AS "CampusName",
                        p2.academic_system_id AS "AcademicSystemId",
                        p2.code AS "AcademicSystemCode",
                        p2.name AS "AcademicSystemName"
                    FROM academic.grade_level AS entity
                    LEFT JOIN org.campus AS p1
                        ON p1.campus_id = entity.campus_id
                    LEFT JOIN academic.academic_system AS p2
                        ON p2.academic_system_id = entity.academic_system_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.grade_level_id = @Id
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

    public sealed class Handler(IGetGradeLevelByIdQuery query)
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
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(GradeLevelEntity))));
            }
            return Result<Response>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById("academics", "grade-level"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetGradeLevelById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantTeacher);
        return endpoints;
    }
}
