using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Workflow.Contracts;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.Modules.Workflow.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Workflow.Features.Approval;

public static class GetApprovalById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ApprovalResponse>>;

    public sealed class Handler(IApprovalQuery entityQuery)
        : IRequestHandler<Query, Result<ApprovalResponse>>
    {
        public async Task<Result<ApprovalResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ApprovalResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Approval))));
            }
            return Result<ApprovalResponse>.Success(ApprovalResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "approval"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ApprovalResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetApprovalById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
