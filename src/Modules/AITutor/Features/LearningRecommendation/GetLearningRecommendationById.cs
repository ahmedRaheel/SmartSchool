using SmartSchool.Modules.AITutor;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.LearningRecommendation;

public static class GetLearningRecommendationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        ILearningRecommendationQuery query)
    {
        public async Task<Result<LearningRecommendation>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<LearningRecommendation>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(LearningRecommendation))));
            }

            return Result<LearningRecommendation>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aitutor/learning-recommendation/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(tenantId, id);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetLearningRecommendationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
