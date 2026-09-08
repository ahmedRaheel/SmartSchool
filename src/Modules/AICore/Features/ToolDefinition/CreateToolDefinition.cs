using SmartSchool.Modules.AICore.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.AICore.Features.ToolDefinition;

public static class CreateToolDefinition
{
    /// <summary>
    /// Represents the response returned by this ToolDefinitionEntity feature.
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

    public interface ICreateToolDefinitionCommand
    {
        Task AddAsync(
                ToolDefinitionEntity entity,
                CancellationToken cancellationToken);
}

    internal sealed class CreateToolDefinitionCommand(IAICoreDbContext dbContext) : ICreateToolDefinitionCommand
    {
        public async Task AddAsync(
                ToolDefinitionEntity entity,
                CancellationToken cancellationToken)
            {
                await dbContext.ToolDefinitions
                    .AddAsync(entity, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
    }

    public sealed class Handler(ICreateToolDefinitionCommand command, IBusinessNumberGenerator numberGenerator)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var code = await numberGenerator.NextAsync("ToolDefinition", "TD", request.TenantId, 3, cancellationToken);

            var entity = ToolDefinitionEntity.Create(
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
                ApiRoutes.EntityCollection(ModuleConstants.RouteSegment, "tool-definition"),
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("CreateToolDefinition")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(ToolDefinitionEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.ToolDefinitionId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
