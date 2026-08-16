using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Features.WorkflowDefinition;

public static class GetWorkflowDefinitionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<WorkflowDefinition> repository)
    {
        public async Task<Result<WorkflowDefinition>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<WorkflowDefinition>.Failure(
                    Error.NotFound("WorkflowDefinition was not found."));
            }

            return Result<WorkflowDefinition>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/workflow/workflow-definition/{id:guid}",
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
            .WithName("GetWorkflowDefinitionById")
            .WithTags("Workflow")
            .RequireAuthorization();

        return endpoints;
    }
}
