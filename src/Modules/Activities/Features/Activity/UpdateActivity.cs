using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using FluentValidation;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Activity;

public static class UpdateActivity
{
    /// <summary>
    /// Represents the response returned by this ActivityEntity feature.
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
        Guid Id,
        string Name) : IRequest<Result<Response>>;

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        }
    }

    public interface IUpdateActivityCommand
    {
        Task UpdateAsync(
                ActivityEntity entity,
                CancellationToken cancellationToken);
Task<ActivityEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class UpdateActivityCommand(IActivitiesDbContext dbContext) : IUpdateActivityCommand
    {
        public async Task UpdateAsync(
                ActivityEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext.Activities
                    .Update(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public async Task<ActivityEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                return await dbContext.Activities
                    .FirstOrDefaultAsync(
                        x => x.TenantId == tenantId
                            && x.ActivityId == id,
                        cancellationToken);
            }
    }

    public sealed class Handler(IUpdateActivityCommand command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var entity = await command.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(ActivityEntity))));
            }


            entity.UpdateDetails(
                entity.Code,
                request.Name);
            await command.UpdateAsync(entity, cancellationToken);
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "activity"),
                async (Guid id, Request request, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var command = request with { Id = id };
                    var result = await mediator.SendAsync<Request, Result<Response>>(
                        command, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("UpdateActivity")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }

    private static Response MapResponse(ActivityEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.ActivityId,
            entity.Code,
            entity.Name,
            entity.MetadataJson);
    }
}
