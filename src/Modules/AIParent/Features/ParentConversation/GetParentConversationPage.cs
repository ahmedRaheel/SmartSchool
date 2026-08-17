using SmartSchool.Application.Messaging;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.AIParent.Contracts;
using SmartSchool.Modules.AIParent.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIParent.Features.ParentConversation;

public static class GetParentConversationPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25) : IRequest<Result<PagedResult<ParentConversationResponse>>>;

    public sealed class Handler(IParentConversationQuery entityQuery)
        : IRequestHandler<Query, Result<PagedResult<ParentConversationResponse>>>
    {
        public async Task<Result<PagedResult<ParentConversationResponse>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(request.Page, request.PageSize);
            var page = await entityQuery.GetPageAsync(
                request.TenantId,
                pageRequest.NormalizedPage,
                pageRequest.NormalizedPageSize,
                cancellationToken);
            var response = new PagedResult<ParentConversationResponse>(
                page.Items.Select(ParentConversationResponse.FromEntity).ToArray(),
                page.Page,
                page.PageSize,
                page.TotalCount);
            return Result<PagedResult<ParentConversationResponse>>.Success(response);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "parent-conversation"),
                async (Guid tenantId, int page, int pageSize, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, page, pageSize);
                    var result = await mediator.SendAsync<Query, Result<PagedResult<ParentConversationResponse>>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetParentConversationPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
