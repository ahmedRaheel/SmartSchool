using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Features.ConversationParticipant;

public static class GetConversationParticipantPage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25);

    public sealed class Handler(
        IRepository<ConversationParticipant> repository)
    {
        public async Task<Result<PagedResult<ConversationParticipant>>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(
                query.Page,
                query.PageSize);

            var result = await repository.GetPageAsync(
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
                "/api/communication/conversation-participant",
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
            .WithTags("Communication")
            .RequireAuthorization();

        return endpoints;
    }
}
