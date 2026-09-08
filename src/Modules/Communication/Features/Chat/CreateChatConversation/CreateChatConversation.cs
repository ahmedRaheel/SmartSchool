using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.Modules.Communication.Persistence;

namespace SmartSchool.Modules.Communication.Features.Chat.CreateChatConversation;

public static class CreateChatConversation
{
    public sealed record ParticipantRequest(Guid UserId, string Role);
    public sealed record Request(Guid? TenantId, string Title, string Type, IReadOnlyCollection<ParticipantRequest> Participants) : IRequest<Response?>;
    public sealed record Response(Guid TenantId, Guid ConversationId, string Title, string ConversationType);
    public interface ICreateChatConversationCommand { Task<ChatConversationEntity> ExecuteAsync(Guid tenantId, Guid userId, Request request, CancellationToken cancellationToken); }
    internal sealed class CreateChatConversationCommand(ICommunicationDbContext dbContext) : ICreateChatConversationCommand
    {
        public async Task<ChatConversationEntity> ExecuteAsync(Guid tenantId, Guid userId, Request request, CancellationToken cancellationToken)
        {
            var entity = ChatConversationEntity.Create(tenantId, request.Title.Trim(), request.Type, userId);
            await dbContext.ChatConversations.AddAsync(entity, cancellationToken);
            foreach (var participant in request.Participants.Append(new ParticipantRequest(userId, "Creator")).GroupBy(x => x.UserId).Select(x => x.First()))
                await dbContext.ChatParticipants.AddAsync(ChatParticipantEntity.Create(tenantId, entity.ChatConversationId, participant.UserId, participant.Role), cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
    public sealed class Handler(ITenantScope tenantScope, ICreateChatConversationCommand command) : IRequestHandler<Request, Response?>
    {
        public async Task<Response?> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.IsSuperAdmin ? request.TenantId : tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue || string.IsNullOrWhiteSpace(request.Title)) return null;
            var entity = await command.ExecuteAsync(tenantId.Value, tenantScope.UserId, request, cancellationToken);
            return new(entity.TenantId, entity.ChatConversationId, entity.Title, entity.ConversationType);
        }
    }
    public static void MapEndpoint(IEndpointRouteBuilder endpoints) => endpoints.MapPost("/api/communication/chat/conversations", async (Request request, IMediator mediator, CancellationToken cancellationToken) =>
    {
        var response = await mediator.SendAsync<Request, Response?>(request, cancellationToken);
        return response is null ? Results.BadRequest(new { message = "Tenant and title are required." }) : Results.Created($"/api/communication/chat/conversations/{response.ConversationId}", response);
    }).WithTags("Communication - Chat").RequireAuthorization();
}
