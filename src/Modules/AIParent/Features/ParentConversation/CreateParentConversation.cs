using SmartSchool.Modules.AIParent.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AIParent.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AIParent.Features.ParentConversation;

public static class CreateParentConversation
{
    /// <summary>
    /// Represents the response returned by this ParentConversationEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
    /// <param name="Code">The business code.</param>
    /// <param name="Name">The display name.</param>
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson);

    public sealed record Request(
        Guid TenantId,
        string Name) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public interface ICreateParentConversationCommand
    {
        Task AddAsync(
                ParentConversationEntity entity,
                CancellationToken cancellationToken);
}

    internal sealed class CreateParentConversationCommand(IAIParentDbContext dbContext) : ICreateParentConversationCommand
    {
        public async Task AddAsync(
                ParentConversationEntity entity,
                CancellationToken cancellationToken)
            {
                await dbContext.ParentConversations
                    .AddAsync(entity, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
    }

    public sealed class Handler(ICreateParentConversationCommand command, IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var code = await numberGenerator.NextAsync("ParentConversation", "PC", request.TenantId, 3, cancellationToken);

            var entity = ParentConversationEntity.Create(
                request.TenantId,
                code,
                request.Name);

            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "parent-conversation"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateParentConversation")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(
        ParentConversationEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.ParentConversationId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
