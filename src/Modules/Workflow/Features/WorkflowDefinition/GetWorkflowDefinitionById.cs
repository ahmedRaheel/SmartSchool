using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Contracts;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.WorkflowDefinition;

public static class GetWorkflowDefinitionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<WorkflowDefinitionResponse>>;

    public sealed class Handler(IWorkflowDefinitionQuery entityQuery)
        : IRequestHandler<Query, Result<WorkflowDefinitionResponse>>
    {
        public async Task<Result<WorkflowDefinitionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<WorkflowDefinitionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(WorkflowDefinition))));
            }
            return Result<WorkflowDefinitionResponse>.Success(WorkflowDefinitionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "workflow-definition"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<WorkflowDefinitionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetWorkflowDefinitionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
