using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

public static class DeleteNotification
{
    public sealed record Command(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid Id);

    public interface IDeleteNotification
    {
        Task<NotificationEntity?> GetByIdAsync(
            Guid tenantId,
            Guid id,
            CancellationToken cancellationToken);

        Task DeleteAsync(
                NotificationEntity entity,
                CancellationToken cancellationToken);

    }

    internal sealed class DeleteNotificationPersistence(
        ICommunicationDbContext dbContext) : IDeleteNotification
    {
        public Task<NotificationEntity?> GetByIdAsync(
            Guid tenantId,
            Guid id,
            CancellationToken cancellationToken)
        {
            return dbContext.Notifications
                .SingleOrDefaultAsync(
                    entity => entity.TenantId == tenantId && entity.NotificationId == id,
                    cancellationToken);
        }

        public async Task DeleteAsync(
                NotificationEntity entity,
                CancellationToken cancellationToken)
            {
                dbContext.Notifications
                    .Remove(entity);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
    }

    public sealed class Handler(IDeleteNotification dataAccess)
        : IRequestHandler<Command, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Command request,
            CancellationToken cancellationToken)
        {
            var entity = await dataAccess.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(NotificationEntity))));
            }
            await dataAccess.DeleteAsync(entity, cancellationToken);
            return Result<Response>.Success(new Response(request.TenantId, request.Id));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "notification"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Command(tenantId, id);
                    var result = await mediator.SendAsync<Command, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("DeleteNotification")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminTenantAdmin);
        return endpoints;
    }
}
