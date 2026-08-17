using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Contracts;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Message;

public static class GetMessageById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<MessageResponse>>;

    public sealed class Handler(IMessageQuery entityQuery)
        : IRequestHandler<Query, Result<MessageResponse>>
    {
        public async Task<Result<MessageResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<MessageResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(Message))));
            }
            return Result<MessageResponse>.Success(MessageResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "message"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<MessageResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetMessageById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
