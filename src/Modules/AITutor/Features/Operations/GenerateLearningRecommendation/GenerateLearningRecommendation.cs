using System.Text.Json;
using SmartSchool.Application.AI;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.Modules.AITutor.Persistence;
namespace SmartSchool.Modules.AITutor.Features.Operations.GenerateLearningRecommendation;
public static class GenerateLearningRecommendation
{
 public sealed record Request(Guid? TenantId,Guid StudentId,string Subject,string Topic,double MasteryScore):IRequest<Response?>; public sealed record Response(Guid RecommendationId,string Recommendation);
 public interface IGenerateLearningRecommendationCommand{Task<LearningRecommendationEntity>AddAsync(Guid tenantId,Request request,string recommendation,CancellationToken cancellationToken);}
 internal sealed class GenerateLearningRecommendationCommand(IAITutorDbContext dbContext):IGenerateLearningRecommendationCommand{public async Task<LearningRecommendationEntity>AddAsync(Guid tenantId,Request request,string recommendation,CancellationToken cancellationToken){var entity=LearningRecommendationEntity.Create(tenantId,$"REC-{Guid.NewGuid():N}",$"{request.Subject} recommendation",JsonSerializer.Serialize(new{request.StudentId,request.Subject,request.Topic,request.MasteryScore,recommendation}));await dbContext.LearningRecommendations.AddAsync(entity,cancellationToken);await dbContext.SaveChangesAsync(cancellationToken);return entity;}}
 public sealed class Handler(ITenantScope tenantScope,IGenerateLearningRecommendationCommand command,IOllamaClient ollama):IRequestHandler<Request,Response?>{public async Task<Response?>HandleAsync(Request request,CancellationToken cancellationToken){var tenantId=tenantScope.IsSuperAdmin?request.TenantId:tenantScope.Resolve(request.TenantId);if(!tenantId.HasValue)return null;var(answer,_)=await ollama.GenerateAsync($"Create a concise learning plan for {request.Subject}/{request.Topic}. Current mastery is {request.MasteryScore:P0}.",cancellationToken);var entity=await command.AddAsync(tenantId.Value,request,answer,cancellationToken);return new(entity.LearningRecommendationId,answer);}}
 public static void MapEndpoint(IEndpointRouteBuilder endpoints)=>endpoints.MapPost("/api/aitutor/operations/recommendations/generate",async(Request request,IMediator mediator,CancellationToken cancellationToken)=>{var response=await mediator.SendAsync<Request,Response?>(request,cancellationToken);return response is null?Results.BadRequest(new{message="Tenant required."}):Results.Ok(response);}).WithTags("AI Tutor Operations").RequireAuthorization();
}
