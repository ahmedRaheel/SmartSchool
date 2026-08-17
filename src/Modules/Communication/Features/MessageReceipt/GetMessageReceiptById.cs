using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Contracts;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.MessageReceipt;

public static class GetMessageReceiptById
{
    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<MessageReceiptResponse>>;

    public sealed class Handler(IMessageReceiptQuery entityQuery)
        : IRequestHandler<Query, Result<MessageReceiptResponse>>
    {
        public async Task<Result<MessageReceiptResponse>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<MessageReceiptResponse>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(MessageReceipt))));
            }
            return Result<MessageReceiptResponse>.Success(MessageReceiptResponse.FromEntity(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "message-receipt"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<MessageReceiptResponse>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetMessageReceiptById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
