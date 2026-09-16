using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.Modules.Activities.Persistence;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Activities.Features.Award;

public static class DeleteAward
{
    public sealed record Request(Guid Id, Guid? TenantId) : IRequest<Result>;

    public interface IDeleteAward
    {
        Task<AwardEntity?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
        Task SaveAsync(CancellationToken cancellationToken);
    }

    internal sealed class DeleteAwardCommand(IActivitiesDbContext dbContext) : IDeleteAward
    {
        public Task<AwardEntity?> GetAsync(Guid tenantId, Guid id, CancellationToken cancellationToken) =>
            dbContext.Awards.SingleOrDefaultAsync(x => x.TenantId == tenantId && x.StudentAwardId == id && x.IsActive, cancellationToken);

        public Task SaveAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
    }

    public sealed class Handler(IDeleteAward command, ITenantScope tenantScope) : IRequestHandler<Request, Result>
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
                return Result.Failure(Error.NotFound("Award was not found."));
            }

            entity.Deactivate();
            await command.SaveAsync(cancellationToken);
            return Result.Success();
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "award"),
                async (Guid id, Guid? tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result>(new Request(id, tenantId), cancellationToken)).ToHttpResult())
            .WithName("DeleteAward")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.AcademicManagement);

        return endpoints;
    }
}
