using System.Text.Json;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.ML;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionSuite;

public static class ForecastPrediction
{
    public sealed record Request(PredictionKind Kind, ForecastPredictionRequest Input) : IRequest<Response>;
    public sealed record Response(PredictionKind Kind, IReadOnlyList<ForecastPoint> Points, decimal Confidence, string ModelVersion, bool UsedMachineLearning);

    public sealed class Handler(IPredictionSuiteService service) : IRequestHandler<Request, Response>
    {
        public async Task<Response> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var result = await service.ForecastAsync(request.Kind, request.Input, cancellationToken);
            return new Response(result.Kind, result.Points, result.Confidence, result.ModelVersion, result.UsedMachineLearning);
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/aiprediction/forecast/{predictionKind}", async (PredictionKind predictionKind, ForecastPredictionRequest request, IMediator mediator, CancellationToken cancellationToken) => Results.Ok(await mediator.SendAsync<Request, Response>(new Request(predictionKind, request), cancellationToken))).WithTags("AI Prediction").RequireAuthorization();
    }
}
