using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.TeachingRecommendation;

public static class GetTeachingRecommendationPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<TeachingRecommendationResponse>>>;

    public sealed class Handler(ITeachingRecommendationQuery entityQuery)
        : IRequestHandler<Query, Result<PagedResult<TeachingRecommendationResponse>>>
    {
        public async Task<Result<PagedResult<TeachingRecommendationResponse>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(request.Page, request.PageSize);
            var page = await entityQuery.GetPageAsync(
                request.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);
            var response = new PagedResult<TeachingRecommendationResponse>(
                page.Items.Select(TeachingRecommendationResponse.FromEntity).ToArray(),
                page.Page,
                page.PageSize,
                page.TotalCount);
            return Result<PagedResult<TeachingRecommendationResponse>>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "teaching-recommendation"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<TeachingRecommendationResponse>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetTeachingRecommendationPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
