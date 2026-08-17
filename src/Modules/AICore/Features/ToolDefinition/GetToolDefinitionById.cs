using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Contracts;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.ToolDefinition;

public static class GetToolDefinitionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ToolDefinitionResponse>>;

    public sealed class Handler(IToolDefinitionQuery entityQuery)
        : IRequestHandler<Query, Result<ToolDefinitionResponse>>
    {
        public async Task<Result<ToolDefinitionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ToolDefinitionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ToolDefinition))));
            }
            return Result<ToolDefinitionResponse>.Success(ToolDefinitionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "tool-definition"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ToolDefinitionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetToolDefinitionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
