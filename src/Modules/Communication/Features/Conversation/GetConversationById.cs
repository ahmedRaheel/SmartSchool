using SmartSchool.Modules.Communication;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Conversation;

public static class GetConversationById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IConversationQuery query)
    {
        public async Task<Result<Conversation>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<Conversation>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Conversation))));
            }

            return Result<Conversation>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/communication/conversation/{id:guid}",
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
            .WithName("GetConversationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
