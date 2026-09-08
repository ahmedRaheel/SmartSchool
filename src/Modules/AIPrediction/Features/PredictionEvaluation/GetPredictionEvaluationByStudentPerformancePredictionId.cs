using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvaluation;

public static class GetPredictionEvaluationByStudentPerformancePredictionId
{
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson,
    Guid StudentExamResultId,
    string? StudentExamResultCode,
    string? StudentExamResultName,
    Guid StudentPerformancePredictionId,
    string? StudentPerformancePredictionCode,
    string? StudentPerformancePredictionName);

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetPredictionEvaluationByStudentPerformancePredictionIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetPredictionEvaluationByStudentPerformancePredictionIdQuery(IDbConnectionFactory connectionFactory)
        : IGetPredictionEvaluationByStudentPerformancePredictionIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                    SELECT
                        tenant_id AS "TenantId",
                        entity.prediction_evaluation_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.student_exam_result_id AS "StudentExamResultId",
                        p1.code AS "StudentExamResultCode",
                        p1.name AS "StudentExamResultName",
                        p2.student_performance_prediction_id AS "StudentPerformancePredictionId",
                        p2.code AS "StudentPerformancePredictionCode",
                        p2.name AS "StudentPerformancePredictionName"
                    FROM ai.prediction_evaluation AS entity
                    LEFT JOIN exam.student_exam_result AS p1
                        ON p1.student_exam_result_id = entity.student_exam_result_id
                    LEFT JOIN ai.student_performance_prediction AS p2
                        ON p2.student_performance_prediction_id = entity.student_performance_prediction_id
                    WHERE tenant_id = @TenantId
                      AND entity.student_performance_prediction_id = @ParentId
                      AND is_active = TRUE;
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

    public sealed class Handler(IGetPredictionEvaluationByStudentPerformancePredictionIdQuery query)
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
                "/api/aiprediction/prediction-evaluation/by-student-performance-prediction/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPredictionEvaluationByStudentPerformancePredictionId")
            .WithTags("AIPrediction")
            .RequireAuthorization();

        return endpoints;
    }
}
