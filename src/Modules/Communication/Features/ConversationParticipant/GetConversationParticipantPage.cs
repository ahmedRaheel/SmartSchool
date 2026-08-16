using SmartSchool.Modules.Communication;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.ConversationParticipant;

public static class GetConversationParticipantPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25);

    public sealed class Handler(
        IConversationParticipantQuery query)
    {
        public async Task<Result<PagedResult<ConversationParticipant>>> HandleAsync(
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

            return Result<PagedResult<ConversationParticipant>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "conversation-participant"),
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
            .WithName("GetConversationParticipantPage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
