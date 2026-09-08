using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Communication.Features.Chat.GetChatConversations;

public static class GetChatConversations
{
    public sealed record Request : IRequest<IReadOnlyList<Response>>;
    public sealed record Response(Guid TenantId, Guid ConversationId, string Title, string ConversationType, Guid CreatedByUserId, bool IsClosed);

    public interface IGetChatConversationsQuery
    {
        Task<IReadOnlyList<Response>> ExecuteAsync(Guid? tenantId, Guid userId, CancellationToken cancellationToken);
    }

    internal sealed class GetChatConversationsQuery(IDbConnectionFactory connectionFactory) : IGetChatConversationsQuery
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(Guid? tenantId, Guid userId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT c.tenant_id AS "TenantId", c.chat_conversation_id AS "ConversationId", c.title AS "Title",
                       c.conversation_type AS "ConversationType", c.created_by_user_id AS "CreatedByUserId", c.is_closed AS "IsClosed"
                FROM communication.chat_conversation c
                JOIN communication.chat_participant p ON p.conversation_id=c.chat_conversation_id AND p.tenant_id=c.tenant_id
                WHERE p.user_id=@UserId AND p.is_active=true AND c.is_active=true
                  AND (@TenantId IS NULL OR c.tenant_id=@TenantId)
                ORDER BY c.created_at DESC
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync<Response>(new CommandDefinition(sql, new { TenantId = tenantId, UserId = userId }, cancellationToken: cancellationToken));
            return rows.AsList();
        }
    }

    public sealed class Handler(ITenantScope tenantScope, IGetChatConversationsQuery query) : IRequestHandler<Request, IReadOnlyList<Response>>
    {
        public Task<IReadOnlyList<Response>> HandleAsync(Request request, CancellationToken cancellationToken) =>
            query.ExecuteAsync(tenantScope.TenantId, tenantScope.UserId, cancellationToken);
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/communication/chat/conversations", async (IMediator mediator, CancellationToken cancellationToken) =>
        Results.Ok(await mediator.SendAsync<Request, IReadOnlyList<Response>>(new Request(), cancellationToken))).WithTags("Communication - Chat").RequireAuthorization();
}
