using Dapper;
using SmartSchool.Application.Http;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Features.Message;

public static class GetMessageByReplyToMessageId
{
    public sealed record Response(
    Guid TenantId,
    Guid Id,
    string Code,
    string Name,
    string? MetadataJson,
    Guid ConversationId,
    string? ConversationCode,
    string? ConversationName,
    Guid? ReplyToMessageId,
    string? ReplyToMessageCode,
    string? ReplyToMessageName);

    public sealed record Query(Guid TenantId, Guid ParentId)
        : IRequest<Result<IReadOnlyCollection<Response>>>;

    public interface IGetMessageByReplyToMessageIdQuery
    {
        Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken);
    }

    internal sealed class GetMessageByReplyToMessageIdQuery(IDbConnectionFactory connectionFactory)
        : IGetMessageByReplyToMessageIdQuery
    {
        public async Task<IReadOnlyCollection<Response>> GetAsync(
            Guid tenantId,
            Guid parentId,
            CancellationToken cancellationToken)
        {
            const string sql = """
                    SELECT
                        tenant_id AS "TenantId",
                        entity.message_id AS "Id",
                        entity.code AS "Code",
                        entity.name AS "Name",
                        entity.metadata_json AS "MetadataJson",
                        p1.conversation_id AS "ConversationId",
                        p1.code AS "ConversationCode",
                        p1.name AS "ConversationName",
                        p2.message_id AS "ReplyToMessageId",
                        p2.code AS "ReplyToMessageCode",
                        p2.name AS "ReplyToMessageName"
                    FROM communication.message AS entity
                    LEFT JOIN communication.conversation AS p1
                        ON p1.conversation_id = entity.conversation_id
                    LEFT JOIN communication.message AS p2
                        ON p2.message_id = entity.reply_to_message_id
                    WHERE tenant_id = @TenantId
                      AND entity.reply_to_message_id = @ParentId
                      AND is_active = TRUE;
                    """;

            await using var connection =
                await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

            var items = await connection.QueryAsync<Response>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, ParentId = parentId },
                    cancellationToken: cancellationToken)).ConfigureAwait(false);

            return items.AsList();
        }
    }

    public sealed class Handler(IGetMessageByReplyToMessageIdQuery query)
        : IRequestHandler<Query, Result<IReadOnlyCollection<Response>>>
    {
        public async Task<Result<IReadOnlyCollection<Response>>> HandleAsync(
            Query request,
            CancellationToken cancellationToken)
        {
            var items = await query.GetAsync(
                request.TenantId,
                request.ParentId,
                cancellationToken).ConfigureAwait(false);

            return Result<IReadOnlyCollection<Response>>.Success(items);
        }
    }

    public static IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/api/communication/message/by-reply-to-message/{parentId:guid}",
                async (Guid parentId, Guid tenantId, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.SendAsync<Query, Result<IReadOnlyCollection<Response>>>(
                        new Query(tenantId, parentId),
                        cancellationToken);
                    return result.ToHttpResult();
                })
            .WithName("GetMessageByReplyToMessageId")
            .WithTags("Communication")
            .RequireAuthorization();

        return endpoints;
    }
}
