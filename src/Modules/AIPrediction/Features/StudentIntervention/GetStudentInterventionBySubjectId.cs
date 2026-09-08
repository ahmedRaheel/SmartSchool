using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.StudentIntervention;

public static class GetStudentInterventionBySubjectId
{
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

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetStudentInterventionBySubjectIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetStudentInterventionBySubjectIdQuery(IDbConnectionFactory connectionFactory)
        : IGetStudentInterventionBySubjectIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
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
                      AND entity.subject_id = @ParentId
                      AND entity.is_active = TRUE;
                    """;

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

            var items = await connection.QueryAsync<Response>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, ParentId = parentId },
                    cancellationToken: cancellationToken)).ConfigureAwait(false);

            return items.AsList();
        }
    }

    public sealed class Handler(IGetStudentInterventionBySubjectIdQuery query)
        : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var items = await query.GetAsync(
                request.TenantId,
                request.ParentId,
                cancellationToken).ConfigureAwait(false);

            return Result<IReadOnlyCollection<Response>>.Success(items);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aiprediction/student-intervention/by-subject/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetStudentInterventionBySubjectId")
            .WithTags("AIPrediction")
            .RequireAuthorization();

        return endpoints;
    }
}
