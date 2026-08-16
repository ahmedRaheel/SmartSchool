using SmartSchool.Modules.AICore;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.ToolDefinition;

public static class DeleteToolDefinition
{
    public sealed record Command(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IToolDefinitionQuery query,
        IToolDefinitionCommand command)
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
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ToolDefinition))));
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
                "/api/aicore/tool-definition/{id:guid}",
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
            .WithName("DeleteToolDefinition")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
