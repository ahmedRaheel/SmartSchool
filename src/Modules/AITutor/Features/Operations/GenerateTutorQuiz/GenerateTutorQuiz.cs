using System.Text.Json;
using SmartSchool.Application.AI;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;
using SmartSchool.Application.Persistence;
namespace SmartSchool.Modules.AITutor.Features.Operations.GenerateTutorQuiz;
public static class GenerateTutorQuiz
{
 public sealed record Request(Guid? TenantId,Guid StudentId,string Subject,string Topic,int QuestionCount=5,string Difficulty="adaptive"):IRequest<Response?>; public sealed record Response(Guid QuizId,object Questions);
 public interface IGenerateTutorQuizCommand{Task<GeneratedQuizEntity>AddAsync(Guid tenantId,Request request,string raw,CancellationToken cancellationToken);}
 internal sealed class GenerateTutorQuizCommand(IAITutorDbContext dbContext):IGenerateTutorQuizCommand{public async Task<GeneratedQuizEntity>AddAsync(Guid tenantId,Request request,string raw,CancellationToken cancellationToken){var entity=GeneratedQuizEntity.Create(tenantId,$"QUIZ-{Guid.NewGuid():N}",$"{request.Subject} - {request.Topic}",JsonSerializer.Serialize(new{request.StudentId,request.Subject,request.Topic,request.Difficulty,questionsJson=raw}));await dbContext.GeneratedQuizs.AddAsync(entity,cancellationToken);await dbContext.SaveChangesAsync(cancellationToken);return entity;}}
 public sealed class Handler(ITenantScope tenantScope,IGenerateTutorQuizCommand command,IOllamaClient ollama,IIntegrationEventPublisher events):IRequestHandler<Request,Response?>{public async Task<Response?>HandleAsync(Request request,CancellationToken cancellationToken){var tenantId=tenantScope.IsSuperAdmin?request.TenantId:tenantScope.Resolve(request.TenantId);if(!tenantId.HasValue)return null;var count=Math.Clamp(request.QuestionCount,1,20);var raw=(await ollama.GenerateAsync($"Generate exactly {count} {request.Difficulty} quiz questions for {request.Subject}, topic {request.Topic}. Return ONLY valid JSON array.",cancellationToken)).Answer;var entity=await command.AddAsync(tenantId.Value,request,raw,cancellationToken);await events.PublishAsync("smartschool.aitutor.quiz-generated",new{tenantId=tenantId.Value,quizId=entity.GeneratedQuizId,request.StudentId},cancellationToken);object questions;try{questions=JsonSerializer.Deserialize<object>(raw)??raw;}catch{questions=raw;}return new(entity.GeneratedQuizId,questions);}}
 public static void MapEndpoint(IEndpointRouteBuilder endpoints)=>endpoints.MapPost("/api/aitutor/operations/quizzes/generate",async(Request request,IMediator mediator,CancellationToken cancellationToken)=>{var response=await mediator.SendAsync<Request,Response?>(request,cancellationToken);return response is null?Results.BadRequest(new{message="Tenant required."}):Results.Created($"/api/aitutor/generated-quiz/{response.QuizId}",response);}).WithTags("AI Tutor Operations").RequireAuthorization();
}
