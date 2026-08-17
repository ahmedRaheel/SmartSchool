using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Contracts;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.WorkflowStep;

public static class GetWorkflowStepById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<WorkflowStepResponse>>;

    public sealed class Handler(IWorkflowStepQuery entityQuery)
        : IRequestHandler<Query, Result<WorkflowStepResponse>>
    {
        public async Task<Result<WorkflowStepResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<WorkflowStepResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(WorkflowStep))));
            }
            return Result<WorkflowStepResponse>.Success(WorkflowStepResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "workflow-step"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<WorkflowStepResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetWorkflowStepById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
