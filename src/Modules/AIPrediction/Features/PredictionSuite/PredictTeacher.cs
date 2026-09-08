using System.Text.Json;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.ML;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionSuite;

public static class PredictTeacher
{
    public sealed record Request(PredictionKind Kind, TeacherPredictionRequest Input) : IRequest<Response>;
    public sealed record Response(PredictionKind Kind, decimal Score, decimal Probability, string RiskLevel, string Outcome, decimal Confidence, string ModelVersion, bool UsedMachineLearning, IReadOnlyList<string> Factors);

    public interface IPredictTeacherCommand
    {
        Task AddAsync(Guid tenantId, PredictionResult result,Request request,  CancellationToken cancellationToken);
    }

    internal sealed class PredictTeacherCommand(IAIPredictionDbContext dbContext) : IPredictTeacherCommand
    {
        public async Task AddAsync(Guid tenantId, PredictionResult result, Request request, CancellationToken cancellationToken)
        {
            var entity = MlPredictionResultEntity.Create(tenantId, result.Kind.ToString(), result.Score, result.Probability, result.RiskLevel, result.Outcome, result.Confidence, result.ModelVersion, result.UsedMachineLearning, JsonSerializer.Serialize(result.Factors), null, null, request.Input.TeacherEmployeeId);
            await dbContext.MlPredictionResults.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public sealed class Handler(IPredictionSuiteService service, IPredictTeacherCommand command) : IRequestHandler<Request, Response>
    {
        public async Task<Response> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var result = await service.PredictTeacherAsync(request.Kind, request.Input, cancellationToken);
            await command.AddAsync(request.Input.TenantId, result, request, cancellationToken);
            return new Response(result.Kind, result.Score, result.Probability, result.RiskLevel, result.Outcome, result.Confidence, result.ModelVersion, result.UsedMachineLearning, result.Factors);
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/aiprediction/teacher/{predictionKind}", async (PredictionKind predictionKind, TeacherPredictionRequest request, IMediator mediator, CancellationToken cancellationToken) => Results.Ok(await mediator.SendAsync<Request, Response>(new Request(predictionKind, request), cancellationToken))).WithTags("AI Prediction").RequireAuthorization();
    }
}
