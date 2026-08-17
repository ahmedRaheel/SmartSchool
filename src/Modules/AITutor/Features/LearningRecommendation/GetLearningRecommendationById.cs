using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Contracts;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.LearningRecommendation;

public static class GetLearningRecommendationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<LearningRecommendationResponse>>;

    public sealed class Handler(ILearningRecommendationQuery entityQuery)
        : IRequestHandler<Query, Result<LearningRecommendationResponse>>
    {
        public async Task<Result<LearningRecommendationResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<LearningRecommendationResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(LearningRecommendation))));
            }
            return Result<LearningRecommendationResponse>.Success(LearningRecommendationResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "learning-recommendation"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<LearningRecommendationResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetLearningRecommendationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
