using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIParent.Contracts;
using SmartSchool.Modules.AIParent.Models;
using SmartSchool.Modules.AIParent.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIParent.Features.ParentMessage;

public static class GetParentMessageById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<ParentMessageResponse>>;

    public sealed class Handler(IParentMessageQuery entityQuery)
        : IRequestHandler<Query, Result<ParentMessageResponse>>
    {
        public async Task<Result<ParentMessageResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<ParentMessageResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ParentMessage))));
            }
            return Result<ParentMessageResponse>.Success(ParentMessageResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "parent-message"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<ParentMessageResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetParentMessageById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
