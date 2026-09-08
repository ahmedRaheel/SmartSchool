using System.Text.Json;
using SmartSchool.Application.AI;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;

namespace SmartSchool.Modules.AITutor.Features.Operations.StartTutorSession;
public static class StartTutorSession
{
    public sealed record Request(Guid? TenantId, Guid StudentId, string Subject, string? Topic) : IRequest<Response?>;
    public sealed record Response(Guid SessionId, Guid ConversationId);
    public interface IStartTutorSessionCommand { Task<Response> ExecuteAsync(Guid tenantId, Request request, CancellationToken cancellationToken); }
    internal sealed class StartTutorSessionCommand(IAITutorDbContext dbContext) : IStartTutorSessionCommand
    {
        public async Task<Response> ExecuteAsync(Guid tenantId, Request request, CancellationToken cancellationToken)
        {
            var session=TutorSessionEntity.Create(tenantId,$"SESSION-{Guid.NewGuid():N}",$"{request.Subject}: {request.Topic}",JsonSerializer.Serialize(request));
            var conversation=TutorConversationEntity.Create(tenantId,$"CONV-{Guid.NewGuid():N}",$"{request.Subject} tutoring",JsonSerializer.Serialize(new{sessionId=session.TutorSessionId,request.StudentId,request.Subject,request.Topic}));
            await dbContext.TutorSessions.AddAsync(session,cancellationToken); await dbContext.TutorConversations.AddAsync(conversation,cancellationToken); await dbContext.SaveChangesAsync(cancellationToken);
            return new(session.TutorSessionId,conversation.TutorConversationId);
        }
    }
    public sealed class Handler(ITenantScope tenantScope,IStartTutorSessionCommand command):IRequestHandler<Request,Response?>
    { public Task<Response?> HandleAsync(Request request,CancellationToken cancellationToken){var tenantId=tenantScope.IsSuperAdmin?request.TenantId:tenantScope.Resolve(request.TenantId);return tenantId.HasValue?Execute():Task.FromResult<Response?>(null); async Task<Response?> Execute()=>await command.ExecuteAsync(tenantId!.Value,request,cancellationToken);}}
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)=>endpoints.MapPost("/api/aitutor/operations/sessions",async(Request request,IMediator mediator,CancellationToken cancellationToken)=>{var response=await mediator.SendAsync<Request,Response?>(request,cancellationToken);return response is null?Results.BadRequest(new{message="Tenant required."}):Results.Created($"/api/aitutor/tutor-session/{response.SessionId}",response);}).WithTags("AI Tutor Operations").RequireAuthorization();
}
