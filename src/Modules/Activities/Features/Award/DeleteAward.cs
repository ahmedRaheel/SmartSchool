using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Award;

public static class DeleteAward
{
    public sealed record Command(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id);

    public interface IDeleteAwardCommand
    {
        Task DeleteAsync(
                AwardEntity entity,
                CancellationToken cancellationToken);

        Task<AwardEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken);

    }

    internal sealed class DeleteAwardCommand(IActivitiesDbContext dbContext) : IDeleteAwardCommand
    {
        public async Task DeleteAsync(
                AwardEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext.Awards
                    .Remove(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }

        public async Task<AwardEntity?> GetByIdAsync(
                Guid tenantId,
                Guid id,
                CancellationToken cancellationToken)
            {
                return await dbContext.Awards
                    .FirstOrDefaultAsync(
                        x => x.TenantId == tenantId
                            && x.StudentAwardId == id,
                        cancellationToken);
            }
    }

    public sealed class Handler(IDeleteAwardCommand command)
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
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(AwardEntity))));
            }
            await command.DeleteAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(request.TenantId, request.Id));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "award"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command(tenantId, id);
                    var result = await mediator.SendAsync<Command, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("DeleteAward")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization();
        return endpoints;
    }
}
