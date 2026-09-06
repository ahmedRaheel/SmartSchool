using SmartSchool.Modules.Communication.Persistence;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using System.Threading.Tasks;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Models;

using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Communication.Features.Notification;

public static class GetNotificationById
{
    /// <summary>
    /// Represents the response returned by this NotificationEntity feature.
    /// </summary>
    /// <param name="TenantId">The owning tenant identifier.</param>
    /// <param name="Id">The entity identifier.</param>
            public sealed record Response(
    Guid TenantId,
    Guid Id,
    Guid RecipientUserId,
    NotificationType Type,
    string Title,
    string Message,
    Guid? RelatedEntityId,
    string? RelatedEntityType,
    string? ActionUrl,
    string Priority,
    bool IsRead,
    DateTimeOffset? ReadAt,
    DateTimeOffset OccurredAt);

    public sealed record Query(
        Guid TenantId,
        Guid Id) : IRequest<Result<Response>>;

    public sealed class Handler(GetNotificationByIdNotificationReadData entityQuery)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var entity = await entityQuery.GetByIdAsync(
                request.TenantId, request.Id, cancellationToken);
            if (entity is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound(ErrorMessages.EntityNotFound(nameof(NotificationEntity))));
            }
            return Result<Response>.Success(MapResponse(entity));
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                ApiRoutes.EntityById(ModuleConstants.RouteSegment, "notification"),
                async (Guid id, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var request = new Query(tenantId, id);
                    var result = await mediator.SendAsync<Query, Result<Response>>(
                        request, cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetNotificationById")
            .WithTags(ModuleConstants.Name)
            .RequireAuthorization(SmartSchoolPolicies.AllAuthenticatedActors);
        return endpoints;
    }

    private static Response MapResponse(
        NotificationEntity entity)
    {
        return new Response(
            entity.TenantId,
            entity.NotificationId,
            entity.RecipientUserId,
            entity.Type,
            entity.Title,
            entity.Message,
            entity.RelatedEntityId,
            entity.RelatedEntityType,
            entity.ActionUrl,
            entity.Priority,
            entity.IsRead,
            entity.ReadAt,
            entity.OccurredAt);
    }
}

/// <summary>
/// Feature-owned data access for GetNotificationById. Do not share across slices.
/// </summary>
internal sealed class GetNotificationByIdNotificationReadData(ICommunicationDbContext dbContext,
    IDbConnectionFactory connectionFactory)
{

    public Task<NotificationEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        return dbContext.Notifications
            .SingleOrDefaultAsync(
                entity =>
                    entity.TenantId == tenantId &&
                    entity.NotificationId == id,
                cancellationToken);
    }
}
