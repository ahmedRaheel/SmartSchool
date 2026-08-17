using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.TeachingRecommendation;

public static class GetTeachingRecommendationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<TeachingRecommendationResponse>>;

    public sealed class Handler(ITeachingRecommendationQuery entityQuery)
        : IRequestHandler<Query, Result<TeachingRecommendationResponse>>
    {
        public async Task<Result<TeachingRecommendationResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<TeachingRecommendationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(TeachingRecommendation))));
            }
            return Result<TeachingRecommendationResponse>.Success(TeachingRecommendationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "teaching-recommendation"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<TeachingRecommendationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTeachingRecommendationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
