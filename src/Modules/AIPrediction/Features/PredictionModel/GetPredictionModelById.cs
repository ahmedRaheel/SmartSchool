using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionModel;

public static class GetPredictionModelById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<PredictionModelResponse>>;

    public sealed class Handler(IPredictionModelQuery entityQuery)
        : IRequestHandler<Query, Result<PredictionModelResponse>>
    {
        public async Task<Result<PredictionModelResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<PredictionModelResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(PredictionModel))));
            }
            return Result<PredictionModelResponse>.Success(PredictionModelResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "prediction-model"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<PredictionModelResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPredictionModelById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
