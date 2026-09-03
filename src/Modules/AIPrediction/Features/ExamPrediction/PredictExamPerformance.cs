using SmartSchool.Modules.AIPrediction.Persistence;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.ML;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.ExamPrediction;

public static class PredictExamPerformance
{
    public sealed record Request(
        Guid TenantId,
        Guid StudentId,
        Guid SubjectId,
        string TargetExamTypeCode,
        Guid? TargetExamId = null,
        Guid? TargetExamSubjectId = null) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid PredictionId,
        Guid StudentId,
        Guid SubjectId,
        string TargetExamTypeCode,
        decimal PredictedMarks,
        decimal PredictedPercentage,
        string PredictedGrade,
        decimal LowerBoundPercentage,
        decimal UpperBoundPercentage,
        decimal Confidence,
        decimal PassProbability,
        string Trend,
        string RiskLevel,
        string ModelVersion,
        int HistoricalResultCount,
        bool UsedMachineLearning);

    public sealed class Handler(
        IExamPredictionService predictionService,
        IAIPredictionDbContext dbContext) : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var prediction = await predictionService.PredictAsync(
                new PredictExamPerformanceRequest(
                    request.TenantId,
                    request.StudentId,
                    request.SubjectId,
                    request.TargetExamTypeCode,
                    request.TargetExamId,
                    request.TargetExamSubjectId),
                cancellationToken);

            var entity = MlExamPredictionEntity.Create(
                request.TenantId,
                request.StudentId,
                request.SubjectId,
                request.TargetExamId,
                request.TargetExamSubjectId,
                prediction.TargetExamTypeCode,
                prediction.PredictedMarks,
                prediction.PredictedPercentage,
                prediction.PredictedGrade,
                prediction.LowerBoundPercentage,
                prediction.UpperBoundPercentage,
                prediction.Confidence,
                prediction.PassProbability,
                prediction.Trend,
                prediction.RiskLevel,
                prediction.ModelVersion,
                prediction.HistoricalResultCount,
                prediction.UsedMachineLearning);

            await dbContext.MlExamPredictions.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result<Response>.Success(new Response(
                entity.PredictionId,
                request.StudentId,
                request.SubjectId,
                prediction.TargetExamTypeCode,
                prediction.PredictedMarks,
                prediction.PredictedPercentage,
                prediction.PredictedGrade,
                prediction.LowerBoundPercentage,
                prediction.UpperBoundPercentage,
                prediction.Confidence,
                prediction.PassProbability,
                prediction.Trend,
                prediction.RiskLevel,
                prediction.ModelVersion,
                prediction.HistoricalResultCount,
                prediction.UsedMachineLearning));
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/api/aiprediction/exam-performance/predict",
            async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken);
                return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);
            })
            .WithName("PredictExamPerformance")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
    }
}
