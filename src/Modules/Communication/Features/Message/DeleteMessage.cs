using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Message;

public static class DeleteMessage
{
    public sealed record Command(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id);

    public interface IDeleteMessageCommand
    {
        Task DeleteAsync(
                MessageEntity entity,
                CancellationToken cancellationToken);

        Task<MessageEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class DeleteMessageCommand(ICommunicationDbContext dbContext) : IDeleteMessageCommand
    {
        public async Task DeleteAsync(
                MessageEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext.Messages
                    .Remove(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public async Task<MessageEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                return await dbContext.Messages
                    .FirstOrDefaultAsync(
                        x => x.TenantId == tenantId
                            && x.MessageId == id,
                        cancellationToken);
            }
    }

    public sealed class Handler(IDeleteMessageCommand command)
        : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Command request,
            CancellationToken cancellationToken)
        {
            var entity = await command.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(MessageEntity))));
            }
            await command.DeleteAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(request.TenantId, request.Id));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "message"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command(tenantId, id);
                    var result = await mediator.SendAsync<Command, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("DeleteMessage")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
