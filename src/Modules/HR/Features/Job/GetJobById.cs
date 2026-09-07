using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.HR.Features.Job;

public static class GetJobById
{
    /// <summary>
    /// Represents the response returned by this JobEntity feature.
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
    Guid? DepartmentId,
    string? DepartmentCode,
    string? DepartmentName,
    Guid? JobFamilyId,
    string? JobFamilyCode,
    string? JobFamilyName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public interface IGetJobByIdQuery
    {
        Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class GetJobByIdQuery(
        IDbConnectionFactory connectionFactory) : IGetJobByIdQuery
    {
        public async Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                const string sql = """
                    SELECT
                        entity.tenant_id AS "TenantId",
                        entity.job_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.department_id AS "DepartmentId",
                        p1.code AS "DepartmentCode",
                        p1.name AS "DepartmentName",
                        p2.job_family_id AS "JobFamilyId",
                        p2.code AS "JobFamilyCode",
                        p2.name AS "JobFamilyName"
                    FROM hr.job AS entity
                    LEFT JOIN org.department AS p1
                        ON p1.department_id = entity.department_id
                    LEFT JOIN hr.job_family AS p2
                        ON p2.job_family_id = entity.job_family_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.job_id = @Id
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

    public sealed class Handler(IGetJobByIdQuery query)
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
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(JobEntity))));
            }
            return Result<Response>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "job"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetJobById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
