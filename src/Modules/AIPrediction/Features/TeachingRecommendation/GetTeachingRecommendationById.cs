using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.TeachingRecommendation;

public static class GetTeachingRecommendationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<TeachingRecommendation> repository)
    {
        public async Task<Result<TeachingRecommendation>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<TeachingRecommendation>.Failure(
                    Error.NotFound("TeachingRecommendation was not found."));
            }

            return Result<TeachingRecommendation>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aiprediction/teaching-recommendation/{id:guid}",
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
            .WithName("GetTeachingRecommendationById")
            .WithTags("AIPrediction")
            .RequireAuthorization();

        return endpoints;
    }
}
