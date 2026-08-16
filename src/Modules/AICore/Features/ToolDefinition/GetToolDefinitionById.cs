using SmartSchool.Modules.AICore;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.ToolDefinition;

public static class GetToolDefinitionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IToolDefinitionQuery query)
    {
        public async Task<Result<ToolDefinition>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<ToolDefinition>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ToolDefinition))));
            }

            return Result<ToolDefinition>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/aicore/tool-definition/{id:guid}",
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
            .WithName("GetToolDefinitionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
