using SmartSchool.Modules.Workflow;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.WorkflowStep;

public static class GetWorkflowStepById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IWorkflowStepQuery query)
    {
        public async Task<Result<WorkflowStep>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await query.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<WorkflowStep>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(WorkflowStep))));
            }

            return Result<WorkflowStep>.Success(entity);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(
        IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/workflow/workflow-step/{id:guid}",
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
            .WithName("GetWorkflowStepById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();

        return endpoints;
    }
}
