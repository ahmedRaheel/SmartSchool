using Dapper;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Communication.Features.Chat.GetChatMessages;

public static class GetChatMessages
{
    public sealed record Request(Guid ConversationId) : IRequest<IReadOnlyList<Response>>;
    public sealed record Response(
        Guid TenantId,
        Guid MessageId,
        Guid ConversationId,
        Guid SenderUserId,
        string SenderDisplayName,
        string? SenderRole,
        string Message,
        DateTimeOffset SentAt,
        DateTimeOffset? EditedAt);

    public interface IGetChatMessages
    {
        Task<IReadOnlyList<Response>> ExecuteAsync(Guid? tenantId, Guid userId, Guid conversationId, CancellationToken cancellationToken);
    }

    internal sealed class GetChatMessagesQuery(IDbConnectionFactory connectionFactory) : IGetChatMessages
    {
        public async Task<IReadOnlyList<Response>> ExecuteAsync(Guid? tenantId, Guid userId, Guid conversationId, CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT
                    m.tenant_id AS "TenantId",
                    m.chat_message_id AS "MessageId",
                    m.conversation_id AS "ConversationId",
                    m.sender_user_id AS "SenderUserId",
                    COALESCE(
                        NULLIF(trim(concat_ws(' ', student_sender.first_name, student_sender.last_name)), ''),
                        NULLIF(guardian_sender.full_name, ''),
                        NULLIF(trim(concat_ws(' ', employee_sender.first_name, employee_sender.last_name)), ''),
                        CASE WHEN m.sender_user_id = @UserId THEN 'You' ELSE 'User' END
                    ) AS "SenderDisplayName",
                    sp.role AS "SenderRole",
                    m.message AS "Message",
                    m.sent_at AS "SentAt",
                    m.edited_at AS "EditedAt"
                FROM communication.chat_message m
                JOIN communication.chat_participant reader
                  ON reader.conversation_id=m.conversation_id AND reader.tenant_id=m.tenant_id
                LEFT JOIN communication.chat_participant sp
                  ON sp.conversation_id=m.conversation_id AND sp.tenant_id=m.tenant_id AND sp.user_id=m.sender_user_id AND sp.is_active=true
                LEFT JOIN student.student student_sender
                  ON student_sender.tenant_id = m.tenant_id
                 AND student_sender.user_id = m.sender_user_id
                 AND student_sender.is_active = TRUE
                LEFT JOIN student.guardian guardian_sender
                  ON guardian_sender.tenant_id = m.tenant_id
                 AND guardian_sender.user_id = m.sender_user_id
                 AND guardian_sender.is_active = TRUE
                LEFT JOIN hr.employee employee_sender
                  ON employee_sender.tenant_id = m.tenant_id
                 AND employee_sender.user_id = m.sender_user_id
                 AND employee_sender.is_active = TRUE
                WHERE m.conversation_id=@ConversationId
                  AND reader.user_id=@UserId AND reader.is_active=true
                  AND m.is_active=true AND m.is_deleted=false
                  AND (@TenantId IS NULL OR m.tenant_id=@TenantId)
                ORDER BY m.sent_at;
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var rows = await connection.QueryAsync<Response>(new CommandDefinition(sql, new { TenantId = tenantId, UserId = userId, ConversationId = conversationId }, cancellationToken: cancellationToken));
            return rows.AsList();
        }
    }

    public sealed class Handler(ITenantScope tenantScope, IGetChatMessages query) : IRequestHandler<Request, IReadOnlyList<Response>>
    {
        public Task<IReadOnlyList<Response>> HandleAsync(Request request, CancellationToken cancellationToken) =>
            query.ExecuteAsync(tenantScope.TenantId, tenantScope.UserId, request.ConversationId, cancellationToken);
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/api/communication/chat/conversations/{conversationId:guid}/messages", async (Guid conversationId, IMediator mediator, CancellationToken cancellationToken) =>
                Results.Ok(await mediator.SendAsync<Request, IReadOnlyList<Response>>(new Request(conversationId), cancellationToken)))
            .WithTags("Communication - Chat")
            .RequireAuthorization();
}
