using System.Text.Json;
using SmartSchool.Application.AI;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.Application.Persistence;
using SmartSchool.SharedKernel.Constants;
namespace SmartSchool.Modules.AITutor.Features.Operations.AskTutor;
public static class AskTutor
{
    public sealed record Request(Guid? TenantId,Guid SessionId,Guid StudentId,string Subject,string Topic,string Message):IRequest<Response?>;
    public sealed record Response(Guid MessageId,string Answer,string Model);
    public interface IAskTutorCommand { Task<TutorMessageEntity> AddAsync(Guid tenantId,string role,string payload,CancellationToken cancellationToken); }
    internal sealed class AskTutorCommand(IAITutorDbContext dbContext):IAskTutorCommand { public async Task<TutorMessageEntity>AddAsync(Guid tenantId,string role,string payload,CancellationToken cancellationToken){var entity=TutorMessageEntity.Create(tenantId,$"TMSG-{Guid.NewGuid():N}",role,payload);await dbContext.TutorMessages.AddAsync(entity,cancellationToken);await dbContext.SaveChangesAsync(cancellationToken);return entity;}}
    public sealed class Handler(ITenantScope tenantScope,IAskTutorCommand command,IOllamaClient ollama,IIntegrationEventPublisher events):IRequestHandler<Request,Response?>
    { public async Task<Response?> HandleAsync(Request request,CancellationToken cancellationToken){var tenantId=tenantScope.IsSuperAdmin?request.TenantId:tenantScope.Resolve(request.TenantId);if(!tenantId.HasValue)return null;await command.AddAsync(tenantId.Value,SmartSchoolRoles.Student,JsonSerializer.Serialize(new{request.SessionId,request.StudentId,role="user",content=request.Message,request.Subject,request.Topic}),cancellationToken);var prompt=$"You are SmartSchool AI Tutor. Subject: {request.Subject}. Topic: {request.Topic}. Teach with hints and formative questions. Student: {request.Message}";var(answer,model)=await ollama.GenerateAsync(prompt,cancellationToken);var message=await command.AddAsync(tenantId.Value,"AI Tutor",JsonSerializer.Serialize(new{request.SessionId,request.StudentId,role="assistant",content=answer}),cancellationToken);await events.PublishAsync(KafkaTopics.ChatbotQuestionAsked,new{tenantId=tenantId.Value,bot="student-tutor",request.StudentId,request.SessionId},cancellationToken);return new(message.TutorMessageId,answer,model);}}
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)=>endpoints.MapPost("/api/aitutor/operations/ask",async(Request request,IMediator mediator,CancellationToken cancellationToken)=>{var response=await mediator.SendAsync<Request,Response?>(request,cancellationToken);return response is null?Results.BadRequest(new{message="Tenant required."}):Results.Ok(response);}).WithTags("AI Tutor Operations").RequireAuthorization();
}
