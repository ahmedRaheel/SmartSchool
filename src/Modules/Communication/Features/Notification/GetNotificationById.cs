using Dapper;
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
        Guid Id) : IRequest<Result<Response>>;    public interface IGetNotificationByIdQuery
    {
        Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken);
    }



    internal sealed class GetNotificationByIdQuery(IDbConnectionFactory connectionFactory) : IGetNotificationByIdQuery
    {
        public async Task<Result<Response>> ExecuteAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT tenant_id AS "TenantId", notification_id AS "Id", recipient_user_id AS "RecipientUserId",
                       type AS "Type", title AS "Title", message AS "Message", related_entity_id AS "RelatedEntityId",
                       related_entity_type AS "RelatedEntityType", action_url AS "ActionUrl", priority AS "Priority",
                       is_read AS "IsRead", read_at AS "ReadAt", occurred_at AS "OccurredAt"
                FROM communication.notification
                WHERE tenant_id = @TenantId AND notification_id = @Id AND is_active = TRUE;
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var row = await connection.QuerySingleOrDefaultAsync<NotificationRow>(
                new CommandDefinition(sql, new { request.TenantId, request.Id }, cancellationToken: cancellationToken));
            if (row is null)
            {
                return Result<Response>.Failure(Error.NotFound(ErrorMessages.EntityNotFound(nameof(Response))));
            }
            var response = new Response(row.TenantId, row.Id, row.RecipientUserId,
                Enum.Parse<NotificationType>(row.Type, true), row.Title, row.Message, row.RelatedEntityId,
                row.RelatedEntityType, row.ActionUrl, row.Priority, row.IsRead, row.ReadAt, row.OccurredAt);
            return Result<Response>.Success(response);
        }
    }

    public sealed class Handler(IGetNotificationByIdQuery query)
        : IRequestHandler<Query, Result<Response>>
    {
        public Task<Result<Response>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            return query.ExecuteAsync(request, cancellationToken);
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
    private sealed record NotificationRow(Guid TenantId, Guid Id, Guid RecipientUserId, string Type, string Title, string Message, Guid? RelatedEntityId, string? RelatedEntityType, string? ActionUrl, string Priority, bool IsRead, DateTimeOffset? ReadAt, DateTimeOffset OccurredAt);
}
