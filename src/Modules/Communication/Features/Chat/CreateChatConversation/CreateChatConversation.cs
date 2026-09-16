using Dapper;
using FluentValidation;
using SmartSchool.Application.Http;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.Modules.Communication.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Features.Chat.CreateChatConversation;

public static class CreateChatConversation
{
    public sealed record ParticipantRequest(Guid UserId, string Role);

    public sealed record Request(
        Guid? TenantId,
        string Title,
        string Type,
        IReadOnlyCollection<ParticipantRequest> Participants)
        : IRequest<Result<Response>>;

    public sealed record Response(
        Guid TenantId,
        Guid ConversationId,
        string Title,
        string ConversationType,
        int ParticipantCount);

    public sealed class Validator : AbstractValidator<Request>
    {
        private static readonly string[] Types = ["DIRECT", "GROUP", "BROADCAST"];

        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Type)
                .Must(value => Types.Contains((value ?? string.Empty).Trim().ToUpperInvariant()))
                .WithMessage("Conversation type must be DIRECT, GROUP, or BROADCAST.");
            RuleForEach(x => x.Participants).ChildRules(participant =>
            {
                participant.RuleFor(x => x.UserId).NotEmpty();
                participant.RuleFor(x => x.Role).MaximumLength(50);
            });
        }
    }

    public interface ICreateChatConversation
    {
        Task<ChatConversationEntity> ExecuteAsync(
            Guid tenantId,
            Guid userId,
            Request request,
            CancellationToken cancellationToken);
    }

    internal sealed class CreateChatConversationCommand(ICommunicationDbContext dbContext)
        : ICreateChatConversation
    {
        public async Task<ChatConversationEntity> ExecuteAsync(
            Guid tenantId,
            Guid userId,
            Request request,
            CancellationToken cancellationToken)
        {
            var entity = ChatConversationEntity.Create(
                tenantId,
                request.Title.Trim(),
                request.Type.Trim().ToUpperInvariant(),
                userId);

            await dbContext.ChatConversations.AddAsync(entity, cancellationToken);

            var participants = request.Participants
                .Append(new ParticipantRequest(userId, "Creator"))
                .GroupBy(item => item.UserId)
                .Select(group => group.First());

            foreach (var participant in participants)
            {
                await dbContext.ChatParticipants.AddAsync(
                    ChatParticipantEntity.Create(
                        tenantId,
                        entity.ChatConversationId,
                        participant.UserId,
                        string.IsNullOrWhiteSpace(participant.Role) ? "Member" : participant.Role.Trim()),
                    cancellationToken);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }

    public sealed class Handler(
        ITenantScope tenantScope,
        IDbConnectionFactory connectionFactory,
        ICreateChatConversation command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.IsSuperAdmin
                ? request.TenantId
                : tenantScope.Resolve(request.TenantId);

            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var participantIds = request.Participants
                .Select(item => item.UserId)
                .Append(tenantScope.UserId)
                .Distinct()
                .ToArray();

            if (!await ParticipantsExistAsync(tenantId.Value, participantIds, cancellationToken))
            {
                return Result<Response>.Failure(
                    Error.Validation("Every conversation participant must be an active user in the selected tenant."));
            }

            if (request.Type.Equals("DIRECT", StringComparison.OrdinalIgnoreCase) && participantIds.Length != 2)
            {
                return Result<Response>.Failure(
                    Error.Validation("A direct conversation must contain exactly two users."));
            }

            var entity = await command.ExecuteAsync(
                tenantId.Value,
                tenantScope.UserId,
                request,
                cancellationToken);

            return Result<Response>.Success(new Response(
                entity.TenantId,
                entity.ChatConversationId,
                entity.Title,
                entity.ConversationType,
                participantIds.Length));
        }

        private async Task<bool> ParticipantsExistAsync(
            Guid tenantId,
            IReadOnlyCollection<Guid> participantIds,
            CancellationToken cancellationToken)
        {
            const string sql = """
                SELECT COUNT(*)::int
                FROM identity."Users"
                WHERE "TenantId"=@TenantId
                  AND "IsActive"=TRUE
                  AND "Id" = ANY(@ParticipantIds);
                """;

            await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
            var count = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    sql,
                    new { TenantId = tenantId, ParticipantIds = participantIds.ToArray() },
                    cancellationToken: cancellationToken));

            return count == participantIds.Count;
        }
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost(
                "/api/communication/chat/conversations",
                async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
                    (await mediator.SendAsync<Request, Result<Response>>(request, cancellationToken)).ToHttpResult())
            .WithTags("Communication - Chat")
            .RequireAuthorization();
}
