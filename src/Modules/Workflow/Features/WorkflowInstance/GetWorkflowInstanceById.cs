using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Contracts;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.WorkflowInstance;

public static class GetWorkflowInstanceById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<WorkflowInstanceResponse>>;

    public sealed class Handler(IWorkflowInstanceQuery entityQuery)
        : IRequestHandler<Query, Result<WorkflowInstanceResponse>>
    {
        public async Task<Result<WorkflowInstanceResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<WorkflowInstanceResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(WorkflowInstance))));
            }
            return Result<WorkflowInstanceResponse>.Success(WorkflowInstanceResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "workflow-instance"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<WorkflowInstanceResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetWorkflowInstanceById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
