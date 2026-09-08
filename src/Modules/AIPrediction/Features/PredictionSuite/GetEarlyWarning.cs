using System.Text.Json;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.ML;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionSuite;

public static class GetEarlyWarning
{
    public sealed record Request(StudentPredictionRequest Input) : IRequest<Response>;
    public sealed record Response(decimal OverallRiskScore, string RiskLevel, PredictionResult Academic, PredictionResult Attendance, PredictionResult Assignment, PredictionResult Fee, PredictionResult Dropout, PredictionResult Promotion, IReadOnlyList<string> TopFactors);

    public sealed class Handler(IPredictionSuiteService service) : IRequestHandler<Request, Response>
    {
        public async Task<Response> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var result = await service.GetEarlyWarningAsync(request.Input, cancellationToken);
            return new Response(result.OverallRiskScore, result.RiskLevel, result.Academic, result.Attendance, result.Assignment, result.Fee, result.Dropout, result.Promotion, result.TopFactors);
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/aiprediction/early-warning", async (StudentPredictionRequest request, IMediator mediator, CancellationToken cancellationToken) => Results.Ok(await mediator.SendAsync<Request, Response>(new Request(request), cancellationToken))).WithTags("AI Prediction").RequireAuthorization();
    }
}
