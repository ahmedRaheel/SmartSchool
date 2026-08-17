using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvaluation;

public static class GetPredictionEvaluationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<PredictionEvaluationResponse>>;

    public sealed class Handler(IPredictionEvaluationQuery entityQuery)
        : IRequestHandler<Query, Result<PredictionEvaluationResponse>>
    {
        public async Task<Result<PredictionEvaluationResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<PredictionEvaluationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(PredictionEvaluation))));
            }
            return Result<PredictionEvaluationResponse>.Success(PredictionEvaluationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "prediction-evaluation"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<PredictionEvaluationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPredictionEvaluationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
