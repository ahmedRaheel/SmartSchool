using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvidence;

public static class GetPredictionEvidenceById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<PredictionEvidenceResponse>>;

    public sealed class Handler(IPredictionEvidenceQuery entityQuery)
        : IRequestHandler<Query, Result<PredictionEvidenceResponse>>
    {
        public async Task<Result<PredictionEvidenceResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<PredictionEvidenceResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(PredictionEvidence))));
            }
            return Result<PredictionEvidenceResponse>.Success(PredictionEvidenceResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "prediction-evidence"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<PredictionEvidenceResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPredictionEvidenceById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
