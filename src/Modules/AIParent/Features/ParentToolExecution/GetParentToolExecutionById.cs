using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIParent.Contracts;
using SmartSchool.Modules.AIParent.Models;
using SmartSchool.Modules.AIParent.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIParent.Features.ParentToolExecution;

public static class GetParentToolExecutionById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ParentToolExecutionResponse>>;

    public sealed class Handler(IParentToolExecutionQuery entityQuery)
        : IRequestHandler<Query, Result<ParentToolExecutionResponse>>
    {
        public async Task<Result<ParentToolExecutionResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ParentToolExecutionResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ParentToolExecution))));
            }
            return Result<ParentToolExecutionResponse>.Success(ParentToolExecutionResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "parent-tool-execution"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ParentToolExecutionResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetParentToolExecutionById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
