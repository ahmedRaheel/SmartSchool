using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AITutor.Features.TutorConversation;

public static class CreateTutorConversation
{
    /// <summary>
    /// Represents the response returned by this TutorConversationEntity feature.
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
        string Name,
        string? MetadataJson = null) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public interface ICreateTutorConversationCommand
    {
        Task AddAsync(
                TutorConversationEntity entity,
                CancellationToken cancellationToken);
}

    internal sealed class CreateTutorConversationCommand(IAITutorDbContext dbContext) : ICreateTutorConversationCommand
    {
        public async Task AddAsync(
                TutorConversationEntity entity,
                CancellationToken cancellationToken)
            {
                await dbContext.TutorConversations
                    .AddAsync(entity, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
    }

    public sealed class Handler(ICreateTutorConversationCommand command, IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var code = await numberGenerator.NextAsync("TutorConversation", "CONV", request.TenantId, 3, cancellationToken);

            var entity = TutorConversationEntity.Create(
                request.TenantId,
                code,
                request.Name,
                request.MetadataJson);

            await command.AddAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "tutor-conversation"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateTutorConversation")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(TutorConversationEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.TutorConversationId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
