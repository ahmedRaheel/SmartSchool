using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvidence;

public static class GetPredictionEvidenceById
{
    /// <summary>
    /// Represents the response returned by this PredictionEvidenceEntity feature.
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
    Guid StudentPerformancePredictionId,
    string? StudentPerformancePredictionCode,
    string? StudentPerformancePredictionName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public interface IGetPredictionEvidenceByIdQuery
    {
        Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class GetPredictionEvidenceByIdQuery(
        IDbConnectionFactory connectionFactory) : IGetPredictionEvidenceByIdQuery
    {
        public async Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                const string sql = """
                    SELECT
                        tenant_id AS "TenantId",
                        entity.prediction_evidence_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.student_performance_prediction_id AS "StudentPerformancePredictionId",
                        p1.code AS "StudentPerformancePredictionCode",
                        p1.name AS "StudentPerformancePredictionName"
                    FROM ai.prediction_evidence AS entity
                    LEFT JOIN ai.student_performance_prediction AS p1
                        ON p1.student_performance_prediction_id = entity.student_performance_prediction_id
                    WHERE tenant_id = @TenantId
                      AND entity.prediction_evidence_id = @Id
                      AND is_active = TRUE;
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

    public sealed class Handler(IGetPredictionEvidenceByIdQuery query)
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
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(PredictionEvidenceEntity))));
            }
            return Result<Response>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "prediction-evidence"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPredictionEvidenceById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
