using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Features.WorkflowStep;

public static class GetWorkflowStepById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id);

    public sealed class Handler(
        IRepository<WorkflowStep> repository)
    {
        public async Task<Result<WorkflowStep>> HandleAsync(
            Query query,
            CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(
                query.TenantId,
                query.Id,
                cancellationToken);

            if (entity is null)
            {
                return Result<WorkflowStep>.Failure(
                    Error.NotFound("WorkflowStep was not found."));
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
            .WithTags("Workflow")
            .RequireAuthorization();

        return endpoints;
    }
}
