using SmartSchool.Modules.Workflow;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.WorkflowDefinition;

public static class GetWorkflowDefinitionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IWorkflowDefinitionQuery query)
    {
        public async Task<Result<WorkflowDefinition>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<WorkflowDefinition>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(WorkflowDefinition))));
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
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
