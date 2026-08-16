using SmartSchool.Application.Persistence;
using SmartSchool.Application.Requests;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Features.Message;

public static class GetMessagePage
{
    public sealed record Query(
        Guid TenantId,
        int Page = 1,
        int PageSize = 25);

    public sealed class Handler(
        IRepository<Message> repository)
    {
        public async Task<Result<PagedResult<Message>>> HandleAsync(
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

            return Result<PagedResult<Message>>.Success(result);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/communication/message",
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
            .WithName("GetMessagePage")
            .WithTags("Communication")
            .RequireAuthorization();

        return endpoints;
    }
}
