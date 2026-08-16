using SmartSchool.Modules.AIParent;
using SmartSchool.Modules.AIParent.Persistence;
using SmartSchool.Modules.AIParent.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIParent.Features.ParentConversation;

public static class DeleteParentConversation
{
    public sealed record Command(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IParentConversationQuery query,
        IParentConversationCommand command)
    {
        public async Task<Result<bool>> HandleAsync(
            Command command,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                command.TenantId,
                command.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<bool>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ParentConversation))));
            }

            await command.DeleteAsync(
                entity,
                cancellationToken);

            return Result<bool>.Success(true);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/api/aiparent/parent-conversation/{id:guid}",
                async (
                    Guid id,
                    Guid tenantId,
                    Handler handler,
                    CancellationToken cancellationToken) =>
                {
                    var command = new Command(
                        tenantId,
                        id);

                    var result = await handler.HandleAsync(
                        command,
                        cancellationToken);

                    return result.ToHttpResult();
                })
            .WithName("DeleteParentConversation")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
