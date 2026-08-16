using SmartSchool.Modules.AIPrediction;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.TeachingRecommendation;

public static class GetTeachingRecommendationPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25);

    public sealed class Handler(
        ITeachingRecommendationQuery query)
    {
        public async Task<Result<PagedResult<TeachingRecommendation>>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(
                query.Page,
                query.PageSize);

            var result = await query.GetPageAsync(
                query.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);

            return Result<PagedResult<TeachingRecommendation>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "teaching-recommendation"),
                async (
                    Guid tenantId,
                    int page,
                    int pageSize,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var query = new Query(
                        tenantId,
                        page,
                        pageSize);

                    var result = await handler.HandleAsync(
                        query,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("GetTeachingRecommendationPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
