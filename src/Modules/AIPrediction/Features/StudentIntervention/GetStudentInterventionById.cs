using SmartSchool.Application.Persistence;
using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.StudentIntervention;

public static class GetStudentInterventionById
{
    /// <summary>
    /// Represents the response returned by this StudentInterventionEntity feature.
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
    Guid? CourseOfferingId,
    string? CourseOfferingCode,
    string? CourseOfferingName,
    Guid? SourcePredictionId,
    string? SourcePredictionCode,
    string? SourcePredictionName,
    Guid? SourceRecommendationId,
    string? SourceRecommendationCode,
    string? SourceRecommendationName,
    Guid? SubjectId,
    string? SubjectCode,
    string? SubjectName);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public interface IGetStudentInterventionByIdQuery
    {
        Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class GetStudentInterventionByIdQuery(
        IDbConnectionFactory connectionFactory) : IGetStudentInterventionByIdQuery
    {
        public async Task<Response?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                const string sql = """
                    SELECT
                        entity.tenant_id AS "TenantId",
                        entity.student_intervention_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.course_offering_id AS "CourseOfferingId",
                        p1.code AS "CourseOfferingCode",
                        p1.name AS "CourseOfferingName",
                        p2.student_performance_prediction_id AS "SourcePredictionId",
                        p2.code AS "SourcePredictionCode",
                        p2.name AS "SourcePredictionName",
                        p3.teaching_recommendation_id AS "SourceRecommendationId",
                        p3.code AS "SourceRecommendationCode",
                        p3.name AS "SourceRecommendationName",
                        p4.subject_id AS "SubjectId",
                        p4.code AS "SubjectCode",
                        p4.name AS "SubjectName"
                    FROM ai.student_intervention AS entity
                    LEFT JOIN academic.course_offering AS p1
                        ON p1.course_offering_id = entity.course_offering_id
                    LEFT JOIN ai.student_performance_prediction AS p2
                        ON p2.student_performance_prediction_id = entity.source_prediction_id
                    LEFT JOIN ai.teaching_recommendation AS p3
                        ON p3.teaching_recommendation_id = entity.source_recommendation_id
                    LEFT JOIN academic.subject AS p4
                        ON p4.subject_id = entity.subject_id
                    WHERE entity.tenant_id = @TenantId
                      AND entity.student_intervention_id = @Id
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

    public sealed class Handler(IGetStudentInterventionByIdQuery query)
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
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(StudentInterventionEntity))));
            }
            return Result<Response>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "student-intervention"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentInterventionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
