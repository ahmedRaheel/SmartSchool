using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Communication.Features.Chat.GetChatMessages;

public static class GetChatMessages
{
    public sealed record Request(Guid ConversationId) : IRequest<IReadOnlyList<Response>>;
    public sealed record Response(Guid TenantId, Guid MessageId, Guid ConversationId, Guid SenderUserId, string Message, DateTimeOffset SentAt, DateTimeOffset? EditedAt);
    public interface IGetChatMessagesQuery { Task<IReadOnlyList<Response>> ExecuteAsync(Guid? tenantId, Guid userId, Guid conversationId, CancellationToken cancellationToken); }
    internal sealed class GetChatMessagesQuery(IDbConnectionFactory connectionFactory) : IGetChatMessagesQuery
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(Guid? tenantId, Guid userId, Guid conversationId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT m.tenant_id AS "TenantId", m.chat_message_id AS "MessageId", m.conversation_id AS "ConversationId",
                       m.sender_user_id AS "SenderUserId", m.message AS "Message", m.sent_at AS "SentAt", m.edited_at AS "EditedAt"
                FROM communication.chat_message m
                JOIN communication.chat_participant p ON p.conversation_id=m.conversation_id AND p.tenant_id=m.tenant_id
                WHERE m.conversation_id=@ConversationId AND p.user_id=@UserId AND p.is_active=true AND m.is_active=true AND m.is_deleted=false
                  AND (@TenantId IS NULL OR m.tenant_id=@TenantId)
                ORDER BY m.sent_at
                """;
            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync<Response>(new CommandDefinition(sql, new { TenantId = tenantId, UserId = userId, ConversationId = conversationId }, cancellationToken: cancellationToken));
            return rows.AsList();
        }
    }
    public sealed class Handler(ITenantScope tenantScope, IGetChatMessagesQuery query) : IRequestHandler<Request, IReadOnlyList<Response>>
    {
        public Task<IReadOnlyList<Response>> HandleAsync(Request request, CancellationToken cancellationToken) => query.ExecuteAsync(tenantScope.TenantId, tenantScope.UserId, request.ConversationId, cancellationToken);
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapGet("/api/communication/chat/conversations/{conversationId:guid}/messages", async (Guid conversationId, IMediator mediator, CancellationToken cancellationToken) =>
        Results.Ok(await mediator.SendAsync<Request, IReadOnlyList<Response>>(new Request(conversationId), cancellationToken))).WithTags("Communication - Chat").RequireAuthorization();
}
