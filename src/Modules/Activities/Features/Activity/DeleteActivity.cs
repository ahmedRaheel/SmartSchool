using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Activity;

public static class DeleteActivity
{
    public sealed record Request(Guid Id, Guid? TenantId) : IRequest<Result>;

    public interface IDeleteActivity
    {
        Task<ActivityEntity?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
        Task<bool> HasParticipantsAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }

    internal sealed class DeleteActivityCommand(IActivitiesDbContext dbContext) : IDeleteActivity
    {
        public Task<ActivityEntity?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken) =>
            dbContext.Activities.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.ActivityId == id && x.IsActive, cancellationToken);

        public Task<bool> HasParticipantsAsync(Guid tenantId, Guid id, CancellationToken cancellationToken) =>
            dbContext.StudentActivities.AnyAsync(x => x.TenantId == tenantId && x.ActivityId == id && x.IsActive, cancellationToken);

        public Task SaveAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
    }

    public sealed class Handler(IDeleteActivity command, ITenantScope tenantScope) : IRequestHandler<Request, Result>
    {
        public async Task<Result> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result.Failure(Error.Validation("Tenant context is required."));
            }

            var entity = await command.GetAsync(tenantId.Value, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result.Failure(Error.NotFound("Activity was not found."));
            }

            if (await command.HasParticipantsAsync(tenantId.Value, request.Id, cancellationToken))
            {
                return Result.Failure(Error.Conflict("Remove or close student participation records before deleting this activity."));
            }

            entity.Deactivate();
            await command.SaveAsync(cancellationToken);
            return Result.Success();
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "activity"),
                async (Guid id, Guid? tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result>(new Request(id, tenantId), cancellationToken)).ToHttpResult())
            .WithName("DeleteActivity")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        return endpoints;
    }
}
