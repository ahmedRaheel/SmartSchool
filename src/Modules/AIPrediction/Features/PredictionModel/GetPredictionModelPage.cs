using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.AIPrediction.Contracts;
using SmartSchool.Modules.AIPrediction.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionModel;

public static class GetPredictionModelPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<PredictionModelResponse>>>;

    public sealed class Handler(IPredictionModelQuery entityQuery)
        : IRequestHandler<Query, Result<PagedResult<PredictionModelResponse>>>
    {
        public async Task<Result<PagedResult<PredictionModelResponse>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(request.Page, request.PageSize);
            var page = await entityQuery.GetPageAsync(
                request.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);
            var response = new PagedResult<PredictionModelResponse>(
                page.Items.Select(PredictionModelResponse.FromEntity).ToArray(),
                page.Page,
                page.PageSize,
                page.TotalCount);
            return Result<PagedResult<PredictionModelResponse>>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "prediction-model"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<PredictionModelResponse>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetPredictionModelPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
